﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Croco.Core.Abstractions.Models;
using Jint;
using Newtonsoft.Json;
using Zoo.ServerJs.Consts;
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

        IServiceProvider ServiceProvider { get; }


        /// <summary>
        /// Javascript обработчики
        /// </summary>
        Dictionary<string, JsWorkerDocumentation> JsWorkers { get; }

        /// <summary>
        /// Внешние компоненты
        /// </summary>
        Dictionary<string, ExternalJsComponent> ExternalComponents { get; }

        Dictionary<string, RemoteJsOpenApi> RemoteApis { get; }

        readonly ConcurrentDictionary<string, RemoteJsOpenApiDocs> RemoteApiDocs = new ConcurrentDictionary<string, RemoteJsOpenApiDocs>();


        readonly Func<IServiceProvider, HttpClient> _httpClientFactory;

        /// <summary>
        /// Обработчик Js вызовов
        /// </summary>
        public HandleJsCallWorker JsCallHandler { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="properties"></param>
        public JsExecutor(IServiceProvider serviceProvider, JsExecutorProperties properties)
        {
            JsWorkers = properties.JsWorkers;
            ExternalComponents = properties.ExternalComponents;
            RemoteApis = properties.RemoteApis;
            _httpClientFactory = properties.HttpClientProvider;

            _openApiDocs = CreateDocs();
            JsCallHandler = new HandleJsCallWorker(serviceProvider, JsWorkers, ExternalComponents);
            
            var engine = new Engine()
                .SetValue(JsConsts.InnerApiObjectName, JsCallHandler)
                .SetValue("console", new
                {
                    log = new Action<object[]>(Log)
                });

            properties.EngineAction?.Invoke(engine);

            engine.Execute(ScriptResources.ScriptInit);

            Engine = engine;
            ServiceProvider = serviceProvider;
        }

        private HttpClient GetHttpClient()
        {
            return _httpClientFactory(ServiceProvider);
        }

        private JsOpenApiDocs CreateDocs()
        {
            return new JsOpenApiDocs
            {
                Workers = JsWorkers.Select(x => x.Value).Select(x => new JsOpenApiWorkerDocumentation(x)).ToList(),
                ExternalJsComponents = ExternalComponents.Select(x => x.Value).ToList()
            };
        }
        
        #region Свойства
        
        private readonly List<JsLogggedVariables> Logs = new List<JsLogggedVariables>();

        /// <summary>
        /// Движок JInt
        /// </summary>
        public Engine Engine { get; }
        

        #endregion

        #region Методы

        /// <summary>
        /// Получить документацию по удаленным открытым Js Api
        /// </summary>
        /// <returns></returns>
        public async Task UpdateRemotesDocsAsync()
        {
            var httpClient = GetHttpClient();

            foreach (var remoteApi in RemoteApis)
            {
                RemoteApiDocs[remoteApi.Key] = await GetRemoteDocsViaHttpRequest(httpClient, remoteApi.Value);
            }
        }

        /// <summary>
        /// Получить документацию по удаленным JsOpenApi
        /// </summary>
        /// <returns></returns>
        public List<RemoteJsOpenApiDocs> GetRemoteDocs()
        {
            return RemoteApiDocs.Values.ToList();
        }

        private async Task<RemoteJsOpenApiDocs> GetRemoteDocsViaHttpRequest(HttpClient httpClient, RemoteJsOpenApi remoteApi)
        {

            RemoteJsOpenApiDocs docsRecord = new RemoteJsOpenApiDocs
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
            var res = JsCallHandler.Call(workerName, method, methodParams);
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
            var res = JsCallHandler.CallExternal(componentName, methodName, methodPayload);
            return ZooSerializer.Deserialize<TResult>(res);
        }

        /// <summary>
        /// Вызвать скрипт
        /// </summary>
        /// <param name="jsScript"></param>
        /// <returns></returns>
        public BaseApiResponse<JsScriptExecutedResult> RunScriptDetaiiled(string jsScript)
        {
            var startDate = DateTime.UtcNow;
            Logs.Clear();

            try
            {
                Engine.Execute(jsScript);

                return new BaseApiResponse<JsScriptExecutedResult>(true, "Скрипт выполнен успешно", new JsScriptExecutedResult
                {
                    StartedOnUtc = startDate,
                    FinishOnUtc = DateTime.UtcNow,
                    Logs = Logs,
                });
            }

            catch(Exception ex)
            {
                return new BaseApiResponse<JsScriptExecutedResult>(false, "Ошибка при выполнении скрипта. " + ex.Message, new JsScriptExecutedResult
                {
                    StartedOnUtc = startDate,
                    FinishOnUtc = DateTime.UtcNow,
                    ExceptionData = new ExcepionData 
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    },
                    Logs = Logs                    
                });
            }
        }
        #endregion

        private void Log(params object[] objs)
        {
            Logs.Add(new JsLogggedVariables 
            {
                LoggedOnUtc = DateTime.UtcNow,
                SerializedVariables = objs.Select(x => new JsSerializedVariable 
                {
                    DataJson = ZooSerializer.Serialize(x),
                    TypeFullName = x.GetType().FullName
                }).ToList()
            });
        }
    }
}