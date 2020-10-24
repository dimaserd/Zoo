using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Abstractions.Models;
using Jint;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Extensions;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Models.OpenApi;
using Zoo.ServerJs.Resources;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Исполнитель кода на Js
    /// </summary>
    public class JsExecutor
    {
        private readonly JsOpenApiDocs _openApiDocs;

        IJsScriptResultStorage Storage { get; }

        JsExecutorComponents Components { get; }

        Action<Engine> EngineAction { get; }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="httpClient"></param>
        /// <param name="storage"></param>
        /// <param name="properties"></param>
        public JsExecutor(IServiceProvider serviceProvider,
            IServerJsHttpClient httpClient,
            IJsScriptResultStorage storage, 
            JsExecutorProperties properties)
        {
            Components = new JsExecutorComponents
            {
                JsWorkers = properties.JsWorkers,
                ExternalComponents = properties.ExternalComponents,
                RemoteApis = new ConcurrentDictionary<string, RemoteJsOpenApi>(properties.RemoteApis),
                ServiceProvider = serviceProvider,
                HttpClient = httpClient
            };
            
            EngineAction = properties.EngineAction;
            _openApiDocs = CreateDocs();
            Storage = storage;
        }

        private JsOpenApiDocs CreateDocs()
        {
            return new JsOpenApiDocs
            {
                Workers = Components.JsWorkers.Select(x => x.Value)
                    .Select(x => new JsOpenApiWorkerDocumentation(x)).ToList(),
                ExternalJsComponents = Components.ExternalComponents
                    .Select(x => x.Value).ToList()
            };
        }
        
        #region Методы

        /// <summary>
        /// Обновить документацию по удаленным открытым Js Api
        /// </summary>
        /// <returns></returns>
        public async Task UpdateRemotesDocsAsync()
        {
            foreach (var remoteApi in Components.RemoteApis)
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
        public BaseApiResponse AddRemoteApi(RemoteJsOpenApi remoteApi)
        {
            var error = string.Format(ExceptionTexts.RemoteApiWithNameAlreadyRegisteredFormat, remoteApi.Name);

            var res = Components.RemoteApis.TryAdd(remoteApi.Name, remoteApi);

            return new BaseApiResponse(res, res ? "Добавлено" : error);
        }

        /// <summary>
        /// Отредактировать удаленное апи
        /// </summary>
        /// <param name="remoteApi"></param>
        /// <returns></returns>
        public BaseApiResponse EditRemoteApi(RemoteJsOpenApi remoteApi)
        {
            var remoteApis = Components.RemoteApis;

            var hostName = remoteApi.Name;

            if (!remoteApis.ContainsKey(hostName))
            {
                return new BaseApiResponse(false, $"Хост не найден по названию '{hostName}'");
            }

            remoteApis.TryRemove(hostName, out var _);
            remoteApis.TryAdd(hostName, remoteApi);

            return new BaseApiResponse(true, $"Данные хоста '{hostName}' обновлены");
        }

        /// <summary>
        /// Удалить удаленное апи
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public BaseApiResponse DeleteRemoteApi(string hostName)
        {
            var remoteApis = Components.RemoteApis;

            if (!remoteApis.ContainsKey(hostName))
            {
                return new BaseApiResponse(false, $"Хост не найден по названию '{hostName}'");
            }

            remoteApis.TryRemove(hostName, out var _);
            return new BaseApiResponse(false, $"Хост с названием '{hostName}' удален");
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
        public JsOpenApiDocs GetDocumentation() => _openApiDocs;

        /// <summary>
        /// Вызвать внутренний сервис, написанный на Js
        /// </summary>
        /// <param name="workerName">название класса рабочего который нужно вызвать</param>
        /// <param name="method">метод который нужно вызвать у данного рабочего</param>
        /// <param name="methodParams">Параметры метода</param>
        public TResult CallWorkerMethod<TResult>(string workerName, string method, params object[] methodParams)
        {
            var res = GetContext().JsCallWorker.Call(workerName, method, methodParams);
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
            var res = GetContext().JsCallWorker.CallExternal(componentName, methodName, methodPayload);
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
                Id = Guid.NewGuid(),
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

            await Storage.AddScriptResult(result);

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
                var result = Components
                    .GetJsWorker(requestModel.WorkerName)
                    .HandleCall(requestModel.MethodName, Components.ServiceProvider, parameters);

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
            return new JsExecutionContext(Components, EngineAction);
        }
        #endregion
    }
}