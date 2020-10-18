﻿using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Resources;
using Zoo.ServerJs.Services;
using Zoo.ServerJs.Services.Properties;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Models
{
    internal class JsExecutionContext
    {
        internal JsExecutionContext(JsExecutionContextProperties properties)
        {
            var jsCallWorker = new HandleJsCallWorker(properties, this);

            var engine = new Engine()
                    .SetValue(JsConsts.InnerApiObjectName, jsCallWorker)
                    .SetValue("console", new
                    {
                        log = new Action<object[]>(Log)
                    });

            properties.EngineAction?.Invoke(engine);

            engine.Execute(ScriptResources.ScriptInit);

            Engine = engine;
            JsCallWorker = jsCallWorker;
            Components = properties.Components;
        }

        /// <summary>
        /// Список логов консиоли
        /// </summary>
        public List<JsLogggedVariables> ConsoleLogs { get; } = new List<JsLogggedVariables>();

        internal JsExecutorComponents Components { get; }

        /// <summary>
        /// Системные логи времени выполнения
        /// </summary>
        public List<JsExecutionLog> ExecutionLogs { get; } = new List<JsExecutionLog>();

        /// <summary>
        /// Движок
        /// </summary>
        public Engine Engine { get; }
        public HandleJsCallWorker JsCallWorker { get; }

        private void Log(params object[] objs)
        {
            ConsoleLogs.Add(new JsLogggedVariables
            {
                LoggedOnUtc = DateTime.UtcNow,
                SerializedVariables = objs?.Select(x => new JsSerializedVariable
                {
                    DataJson = ZooSerializer.Serialize(x),
                    TypeFullName = x.GetType().FullName
                }).ToList() ?? new List<JsSerializedVariable>()
            });
        }
    }
}