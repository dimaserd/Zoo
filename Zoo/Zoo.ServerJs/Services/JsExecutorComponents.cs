using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Models.OpenApi;
using Zoo.ServerJs.Resources;

namespace Zoo.ServerJs.Services
{
    internal class JsExecutorComponents
    {
        /// <summary>
        /// Javascript обработчики
        /// </summary>
        public Dictionary<string, JsWorkerDocumentation> JsWorkers { get; set; }

        /// <summary>
        /// Внешние компоненты
        /// </summary>
        public Dictionary<string, ExternalJsComponent> ExternalComponents { get; set; }

        public ConcurrentDictionary<string, RemoteJsOpenApi> RemoteApis { get; set; }

        public IServiceProvider ServiceProvider { get; set; }
        public IServerJsHttpClient HttpClient { get; set; }

        public ConcurrentDictionary<string, RemoteJsOpenApiDocs> RemoteApiDocs { get; } = new ConcurrentDictionary<string, RemoteJsOpenApiDocs>();

        public JsWorkerDocumentation GetJsWorker(string workerName)
        {
            if (!JsWorkers.ContainsKey(workerName))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.NoWorkerWithNameRegisteredFormat, workerName));
            }

            return JsWorkers[workerName];
        }

        public ExternalJsComponent GetExternalComponent(string componentName)
        {
            if (!ExternalComponents.ContainsKey(componentName))
            {
                throw new InvalidOperationException($"Компонент не найден по указанному названию '{componentName}'");
            }

            return ExternalComponents[componentName];
        }
    }
}