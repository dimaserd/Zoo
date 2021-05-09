using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Contract.Models;
using Jint;
using Microsoft.Extensions.DependencyInjection;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Extensions;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Models.OpenApi;
using Zoo.ServerJs.Resources;
using Zoo.ServerJs.Services.Internal;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Исполнитель кода на Js
    /// </summary>
    public class JsExecutor
    {
        IServiceProvider ServiceProvider { get; }
        IJsScriptTaskStorage Storage { get; }

        JsExecutorComponents Components { get; }

        Action<Engine> EngineAction { get; }
        Action<IServiceProvider> ScopedServiceProviderAction { get; }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="httpClient"></param>
        /// <param name="storage"></param>
        /// <param name="persistedStorage"></param>
        /// <param name="properties"></param>
        public JsExecutor(IServiceProvider serviceProvider,
            IServerJsHttpClient httpClient,
            IJsScriptTaskStorage storage,
            IPersistedStorage persistedStorage,
            JsExecutorProperties properties)
        {
            Components = new JsExecutorComponents(persistedStorage, properties.ExternalComponents, properties.RemoteApis)
            {
                JsWorkers = properties.JsWorkers,
                HttpClient = httpClient
            };

            ScopedServiceProviderAction = properties.ScopedServiceProviderAction;
            EngineAction = properties.EngineAction;
            ServiceProvider = serviceProvider;
            Storage = storage;
        }

        #region Методы

        /// <summary>
        /// Обновить документацию по удаленным открытым Js Api
        /// </summary>
        /// <returns></returns>
        public async Task UpdateRemotesDocsAsync()
        {
            Components.RemoteApiDocs.Clear();
            var remoteApis = await Components.RemoteApis.GetInternalValueAsync();
            foreach (var remoteApi in remoteApis)
            {
                Components.RemoteApiDocs[remoteApi.Key] = await Components.HttpClient.GetRemoteDocsViaHttpRequest(remoteApi.Value);
            }
        }

        /// <summary>
        /// Получить документацию по удаленным JsOpenApi
        /// </summary>
        /// <returns></returns>
        public List<RemoteJsOpenApiDocs> GetRemoteDocs()
        {
            return Components.RemoteApiDocs.Values.ToList();
        }

        /// <summary>
        /// Добавить удаленное Апи
        /// </summary>
        /// <param name="remoteApi"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> AddRemoteApi(RemoteJsOpenApi remoteApi)
        {
            var error = string.Format(ExceptionTexts.RemoteApiWithNameAlreadyRegisteredFormat, remoteApi.Name);

            var remoteApis = Components.RemoteApis;
            var key = remoteApi.Name;
            
            if (await remoteApis.ContainsKeyAsync(key))
            {
                return new BaseApiResponse(false, error);
            }

            await remoteApis.SetValueAsync(key, remoteApi);

            return new BaseApiResponse(true, "Добавлено");
        }

        /// <summary>
        /// Отредактировать удаленное апи
        /// </summary>
        /// <param name="remoteApi"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> EditRemoteApi(RemoteJsOpenApi remoteApi)
        {
            var remoteApis = Components.RemoteApis;

            var hostName = remoteApi.Name;

            if (!await remoteApis.ContainsKeyAsync(hostName))
            {
                return new BaseApiResponse(false, $"Хост не найден по названию '{hostName}'");
            }

            await remoteApis.SetValueAsync(hostName, remoteApi);

            return new BaseApiResponse(true, $"Данные хоста '{hostName}' обновлены");
        }

        /// <summary>
        /// Удалить удаленное апи
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> DeleteRemoteApi(string hostName)
        {
            var remoteApis = Components.RemoteApis;

            if (!await remoteApis.ContainsKeyAsync(hostName))
            {
                return new BaseApiResponse(false, $"Хост не найден по названию '{hostName}'");
            }

            await remoteApis.RemoveAsync(hostName);
            return new BaseApiResponse(true, $"Хост с названием '{hostName}' удален");
        }

        /// <summary>
        /// Получить Http клиента
        /// </summary>
        /// <returns></returns>
        public IServerJsHttpClient GetHttpClient()
        {
            return Components.HttpClient;
        }
        
        /// <summary>
        /// Получить документацию
        /// </summary>
        /// <returns></returns>
        public async Task<JsOpenApiDocs> GetDocumentation()
        {
            return new JsOpenApiDocs
            {
                Workers = Components.JsWorkers.Select(x => x.Value)
                    .Select(x => new JsOpenApiWorkerDocumentation(x)).ToList(),
                ExternalJsComponents = (await Components.ExternalComponents.GetInternalValueAsync())
                    .Select(x => x.Value).ToList()
            };
        }

        /// <summary>
        /// Вызвать внутренний сервис, написанный на Js
        /// </summary>
        /// <param name="workerName">название класса рабочего который нужно вызвать</param>
        /// <param name="method">метод который нужно вызвать у данного рабочего</param>
        /// <param name="methodParams">Параметры метода</param>
        public TResult CallWorkerMethod<TResult>(string workerName, string method, params object[] methodParams)
        {
            using var context = GetContext();
            var res = context.JsCallWorker.Call(workerName, method, methodParams);
            return ZooSerializer.Deserialize<TResult>(res);
        }

        /// <summary>
        /// Вызвать компонент, написанный на Js
        /// </summary>
        /// <param name="componentName">название компонента который нужно вызвать</param>
        /// <param name="methodName">метод который нужно вызвать у данного компонента</param>
        /// <param name="methodPayload">Параметр, который нужно передать в метод компонента</param>
        public TResult CallExternalComponent<TResult>(string componentName, string methodName, object methodPayload)
        {
            using var context = GetContext();
            var res = context.JsCallWorker.CallExternal(componentName, methodName, methodPayload);
            return ZooSerializer.Deserialize<TResult>(res);
        }

        /// <summary>
        /// Вызвать скрипт
        /// </summary>
        /// <param name="jsScript"></param>
        /// <returns></returns>
        public async Task<JsScriptExecutedResult> RunScriptDetaiiled(string jsScript)
        {
            if (string.IsNullOrWhiteSpace(jsScript))
            {
                return new JsScriptExecutedResult("Скрипт не может быть пустой строкой");
            }

            var startDate = DateTime.UtcNow;

            var context = GetContext();

            JsScriptExecutedResult result = new JsScriptExecutedResult
            {
                Script = jsScript,
                StartedOnUtc = startDate
            };

            try
            {
                context.Engine.Execute(jsScript);

                result.IsSucceeded = true;
                result.ErrorMessage = null;
                result.ExceptionData = null;
            }

            catch(Exception ex)
            {
                result.IsSucceeded = false;
                result.ErrorMessage = ex.Message;
                result.ExceptionData = new ExcepionData
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }

            result.FinishedOnUtc = DateTime.UtcNow;
            result.ConsoleLogs = context.ConsoleLogs;
            result.ExecutionLogs = context.ExecutionLogs;

            await Storage.AddResult(result);

            return result;
        }

        /// <summary>
        /// Вызвать метод рабочего класса.
        /// <para></para>
        /// Данный метод не возвращает исключений. Его нужно испльзовать как внешюю точку доступа.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public CallOpenApiWorkerMethodResponse CallWorkerMethod(CallOpenApiWorkerMethod requestModel)
        {
            if(requestModel == null)
            {
                return new CallOpenApiWorkerMethodResponse
                {
                    IsSucceeded = false,
                    ExcepionData = ExcepionData.Create(new Exception("request is null onjcet"))
                };
            }

            var parameters = new JsWorkerMethodCallParametersFromSerialized(requestModel.SerializedParameters);

            try
            {
                using var scope = ServiceProvider.CreateScope();
                var result = Components
                    .GetJsWorker(requestModel.WorkerName, GetContext())
                    .HandleCall(requestModel.MethodName, scope.ServiceProvider, parameters);

                return new CallOpenApiWorkerMethodResponse
                {
                    IsSucceeded = true,
                    ResponseJson = ZooSerializer.Serialize(result.Result)
                };
            }
            catch(Exception ex)
            {
                return new CallOpenApiWorkerMethodResponse
                {
                    IsSucceeded = false,
                    ExcepionData = ExcepionData.Create(ex)
                };
            }
        }

        private JsExecutionContext GetContext()
        {
            return new JsExecutionContext(Components, ServiceProvider.CreateScope(), EngineAction, ScopedServiceProviderAction);
        }
        #endregion
    }
}