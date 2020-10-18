using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.OpenApi;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Services
{
    internal class RemoteJsCaller
    {
        ConcurrentDictionary<string, RemoteJsOpenApiDocs> RemoteDocs { get; }
        IServerJsHttpClientProvider HttpClientProvider { get; }
        JsExecutionContext ExecutionContext { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="remoteDocs"></param>
        /// <param name="httpClientProvider"></param>
        /// <param name="executionContext"></param>
        internal RemoteJsCaller(ConcurrentDictionary<string, RemoteJsOpenApiDocs> remoteDocs, 
            IServerJsHttpClientProvider httpClientProvider, JsExecutionContext executionContext)
        {
            RemoteDocs = remoteDocs;
            HttpClientProvider = httpClientProvider;
            ExecutionContext = executionContext;
        }

        public InvalidOperationException WriteLogAndGetException(JsExecutionLog log)
        {
            ExecutionContext.ExecutionLogs.Add(log);
            return new InvalidOperationException(log.Message);
        }

        /// <summary>
        /// Вызвать внешний сервис, определенный через Js
        /// </summary>
        /// <param name="remoteName"></param>
        /// <param name="workerName"></param>
        /// <param name="methodName"></param>
        /// <param name="methodParams"></param>
        /// <returns></returns>
        public async Task<string> CallRemoteApi(string remoteName, string workerName, string methodName, params dynamic[] methodParams)
        {
            if (!RemoteDocs.ContainsKey(remoteName))
            {
                throw WriteLogAndGetException(new JsExecutionLog
                {
                    LoggedOnUtc = DateTime.UtcNow,
                    EventId = EventIds.CallRemoteApi.RemoteApiNotFound,
                    Message = $"В системе нет зарегистрированного внешнего апи с именем '{remoteName}'",
                    DataJson = null
                });
            }

            var remoteApi = RemoteDocs[remoteName];

            var worker = remoteApi.Docs.Workers.FirstOrDefault(x => x.WorkerName == workerName);

            if(worker == null)
            {
                throw WriteLogAndGetException(new JsExecutionLog
                {
                    LoggedOnUtc = DateTime.UtcNow,
                    EventId = EventIds.CallRemoteApi.NoWorkerWithNameFound,
                    Message = $"Во внешнем апи '{remoteName}' не зарегистрирован рабочий класс с именем '{workerName}'",
                    DataJson = null
                });
            }

            var method = worker.Methods.FirstOrDefault(x => x.MethodName == methodName);

            if(method == null)
            {
                throw new InvalidOperationException($"Во внешнем апи '{remoteName}' в рабочем классе с именем '{workerName}' не обнаружен метож '{methodName}'");
            }

            var requestModel = new CallRemoteOpenApiMethod
            {
                WorkerName = workerName,
                MethodName = methodName,
                SerializedParameters = methodParams?.Select(ZooSerializer.Serialize).ToArray() ?? Array.Empty<string>()
            };

            var callResult = await CallResult(remoteApi, requestModel);

            if (!callResult.IsSucceeded)
            {
                throw WriteLogAndGetException(new JsExecutionLog
                {
                    EventId = EventIds.CallRemoteApi.CallNotSucceeded,
                    LoggedOnUtc = DateTime.UtcNow,
                    Message = "Ошибка при выполнении удалленного запроса",
                    DataJson = ZooSerializer.Serialize(callResult)
                });
            }

            return callResult.ResponseJson;
        }

        private async Task<CallRemoteOpenApiWorkerMethodResponse> CallResult(RemoteJsOpenApiDocs remoteApi, CallRemoteOpenApiMethod requestModel)
        {
            var httpClient = HttpClientProvider.GetHttpClient();

            var json = ZooSerializer.Serialize(requestModel);

            var url = $"{remoteApi.Description.HostUrl}/JsOpenApi/HandleCallFromRemote";

            HttpResponseMessage result;

            try
            {
                result = await httpClient.PostAsync(url, new StringContent(json));
            }
            catch(Exception ex)
            {
                return new CallRemoteOpenApiWorkerMethodResponse
                {
                    IsSucceeded = false,
                    ErrorMessage = ex.Message
                };
            }
            
            var responseRecord = new RemoteApiResponseRecord
            {
                HostName = remoteApi.Description.Name,
                HostUrl = remoteApi.Description.HostUrl,
                RequestUrl = url,
                Request = json,
                ResponseStatusCode = (int)result.StatusCode,
                Response = await result.Content.ReadAsStringAsync()
            };

            ExecutionContext.ExecutionLogs.Add(new JsExecutionLog
            {
                LoggedOnUtc = DateTime.UtcNow,
                EventId = EventIds.CallRemoteApi.CallLogged,
                Message = "Логгирование удаленного запроса",
                DataJson = ZooSerializer.Serialize(responseRecord)
            });

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new CallRemoteOpenApiWorkerMethodResponse
                {
                    IsSucceeded = false,
                    ErrorMessage = "Ответ не 200",
                };
            }

            try
            {
                return ZooSerializer.Deserialize<CallRemoteOpenApiWorkerMethodResponse>(responseRecord.Response);
            }
            catch (Exception ex)
            {
                ExecutionContext.ExecutionLogs.Add(new JsExecutionLog
                {
                    LoggedOnUtc = DateTime.UtcNow,
                    EventId = EventIds.CallRemoteApi.ResponseDeserializationError,
                    Message = "Логгирование удаленного запроса",
                    DataJson = ZooSerializer.Serialize(responseRecord)
                });

                return new CallRemoteOpenApiWorkerMethodResponse
                {
                    IsSucceeded = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}