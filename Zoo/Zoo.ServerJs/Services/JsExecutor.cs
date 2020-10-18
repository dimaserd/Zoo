using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Jint;
using Newtonsoft.Json;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Models.OpenApi;
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
        /// <param name="httpClientProvider"></param>
        /// <param name="storage"></param>
        /// <param name="properties"></param>
        public JsExecutor(IServiceProvider serviceProvider,
            IServerJsHttpClientProvider httpClientProvider,
            IJsScriptResultStorage storage, 
            JsExecutorProperties properties)
        {
            Components = new JsExecutorComponents
            {
                JsWorkers = properties.JsWorkers,
                ExternalComponents = properties.ExternalComponents,
                RemoteApis = properties.RemoteApis,
                ServiceProvider = serviceProvider,
                HttpClientProvider = httpClientProvider
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
            var httpClient = Components.HttpClientProvider.GetHttpClient();

            foreach (var remoteApi in Components.RemoteApis)
            {
                Components.RemoteApiDocs[remoteApi.Key] = await GetRemoteDocsViaHttpRequest(httpClient, remoteApi.Value);
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

        private async Task<RemoteJsOpenApiDocs> GetRemoteDocsViaHttpRequest(HttpClient httpClient, RemoteJsOpenApi remoteApi)
        {
            var docsRecord = new RemoteJsOpenApiDocs
            {
                Description = remoteApi
            };

            try
            {
                using var response = await httpClient.GetAsync($"{remoteApi.HostUrl}/JsOpenApi/GetDocs");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var str = await response.Content.ReadAsStringAsync();
                    var docs = JsonConvert.DeserializeObject<JsOpenApiDocs>(str);

                    docsRecord.Docs = docs;
                    docsRecord.DocsReceivedOnUtc = DateTime.UtcNow;
                    docsRecord.IsDocsReceived = true;
                }
                else
                {
                    docsRecord.IsDocsReceived = false;
                    docsRecord.DocsReceivedOnUtc = DateTime.UtcNow;
                }
            }
            catch(Exception ex)
            {
                docsRecord.IsDocsReceived = false;
                docsRecord.ReceivingException = new ExcepionData
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }

            return docsRecord;
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
        public TResult Call<TResult>(string workerName, string method, params object[] methodParams)
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

            JsScriptExecutedResult result;
            try
            {

                context.Engine.Execute(jsScript);

                result = new JsScriptExecutedResult
                {
                    Id = Guid.NewGuid(),
                    Script = jsScript,
                    IsSucceeded = true,
                    StartedOnUtc = startDate,
                    FinishedOnUtc = DateTime.UtcNow,
                    ConsoleLogs = context.ConsoleLogs,
                };
            }

            catch(Exception ex)
            {
                result = new JsScriptExecutedResult
                {
                    Id = Guid.NewGuid(),
                    Script = jsScript,
                    ErrorMessage = ex.Message,
                    IsSucceeded = false,
                    StartedOnUtc = startDate,
                    FinishedOnUtc = DateTime.UtcNow,
                    ExceptionData = new ExcepionData 
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    },
                    ConsoleLogs = context.ConsoleLogs
                };
            }

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
        public CallRemoteOpenApiWorkerMethodResponse CallWorkerMethod(CallRemoteOpenApiMethod requestModel)
        {
            if(requestModel == null)
            {
                return new CallRemoteOpenApiWorkerMethodResponse
                {
                    IsSucceeded = false,
                    ErrorMessage = "request is null object"
                };
            }

            var parameters = new JsWorkerMethodCallParametersFromSerialized(requestModel.SerializedParameters);

            try
            {
                var result = Components
                    .GetJsWorker(requestModel.WorkerName)
                    .HandleCall(requestModel.MethodName, Components.ServiceProvider, parameters);

                return new CallRemoteOpenApiWorkerMethodResponse
                {
                    IsSucceeded = true,
                    ResponseJson = ZooSerializer.Serialize(result.Result)
                };
            }
            catch(Exception ex)
            {
                return new CallRemoteOpenApiWorkerMethodResponse
                {
                    IsSucceeded = false,
                    ErrorMessage = ex.Message
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