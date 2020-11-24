using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Models.OpenApi;
using Zoo.ServerJs.Resources;

namespace Zoo.ServerJs.Services.Internal
{
    internal class JsExecutorComponents
    {
        private readonly IPersistedStorage _persistedStorage;

        public JsExecutorComponents(IPersistedStorage persistedStorage,
            Dictionary<string, ExternalJsComponent> externalJsComponents, Dictionary<string, RemoteJsOpenApi> remoteJsOpenApis)
        {
            _persistedStorage = persistedStorage;
            ExternalComponents = new PersistedStorageAsyncDictionary<ExternalJsComponent>(_persistedStorage, "ExternalComponents", externalJsComponents);
            RemoteApis = new PersistedStorageAsyncDictionary<RemoteJsOpenApi>(_persistedStorage, "RemoteApis", remoteJsOpenApis);
        }

        /// <summary>
        /// Javascript обработчики
        /// </summary>
        public Dictionary<string, JsWorkerDocumentation> JsWorkers { get; set; }

        /// <summary>
        /// Внешние компоненты
        /// </summary>
        public PersistedStorageAsyncDictionary<ExternalJsComponent> ExternalComponents { get; }

        public PersistedStorageAsyncDictionary<RemoteJsOpenApi> RemoteApis { get; }

        public IServerJsHttpClient HttpClient { get; set; }

        public ConcurrentDictionary<string, RemoteJsOpenApiDocs> RemoteApiDocs { get; } = new ConcurrentDictionary<string, RemoteJsOpenApiDocs>();

        public JsWorkerDocumentation GetJsWorker(string workerName, JsExecutionContext executionContext)
        {
            if (!JsWorkers.ContainsKey(workerName))
            {
                var mes = string.Format(ExceptionTexts.NoWorkerWithNameRegisteredFormat, workerName);
                executionContext.ExecutionLogs.Add(new JsExecutionLog
                {
                    EventIdName = EventIds.JsExecutorComponents.GetJsWorkerNotFound,
                    Message = mes,
                });
                throw new InvalidOperationException(mes);
            }

            executionContext.ExecutionLogs.Add(new JsExecutionLog
            {
                EventIdName = EventIds.JsExecutorComponents.GetJsWorkerFound,
                Message = $"Js рабочий найден по указанному названию '{workerName}'",
            });

            return JsWorkers[workerName];
        }

        public async Task<ExternalJsComponent> GetExternalComponent(string componentName, JsExecutionContext executionContext)
        {
            if (!await ExternalComponents.ContainsKeyAsync(componentName))
            {
                var mes = $"Компонент не найден по указанному названию '{componentName}'";
                executionContext.ExecutionLogs.Add(new JsExecutionLog
                {
                    EventIdName = EventIds.JsExecutorComponents.GetExternalComponentNotFound,
                    Message = mes
                });
                throw new InvalidOperationException(mes);
            }

            executionContext.ExecutionLogs.Add(new JsExecutionLog
            {
                EventIdName = EventIds.JsExecutorComponents.GetJsWorkerFound,
                Message = $"Компонент найден по указанному названию '{componentName}'",
            });

            return await ExternalComponents.GetValueAsync(componentName);
        }
    }
}