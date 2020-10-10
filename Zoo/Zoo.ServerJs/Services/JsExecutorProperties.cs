using System;
using System.Collections.Generic;
using System.Net.Http;
using Jint;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Свойства для javascript исполнителя
    /// </summary>
    public class JsExecutorProperties
    {
        /// <summary>
        /// Действие над движком
        /// </summary>
        public Action<Engine> EngineAction { get; set; }

        /// <summary>
        /// Список обработчиков
        /// </summary>
        public Dictionary<string, JsWorkerDocumentation> JsWorkers { get; set; } = new Dictionary<string, JsWorkerDocumentation>();

        /// <summary>
        /// Внешние компоненты
        /// </summary>
        public Dictionary<string, ExternalJsComponent> ExternalComponents { get; set; } = new Dictionary<string, ExternalJsComponent>();

        /// <summary>
        /// Удаленные апи
        /// </summary>
        public Dictionary<string, RemoteJsOpenApi> RemoteApis { get; set; } = new Dictionary<string, RemoteJsOpenApi>();

        /// <summary>
        /// Функция для предоставления Http клиента
        /// </summary>
        public Func<IServiceProvider, HttpClient> HttpClientProvider { get; set; }
    }
}