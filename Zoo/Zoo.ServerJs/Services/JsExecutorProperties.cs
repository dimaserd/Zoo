using System;
using System.Collections.Generic;
using Jint;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;

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
        public Dictionary<string, IJsWorker> JsWorkers { get; set; } = new Dictionary<string ,IJsWorker>();

        /// <summary>
        /// Внешние компоненты
        /// </summary>
        public Dictionary<string, ExternalJsComponent> ExternalComponents { get; set; } = new Dictionary<string, ExternalJsComponent>();
    }
}