using System;
using System.Collections.Generic;
using Jint;
using Zoo.ServerJs.Abstractions;

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
        public List<IJsWorker> JsWorkers { get; set; }
    }
}