using Jint;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Resources;
using Zoo.ServerJs.Services;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Models
{
    internal class JsExecutionContext : IDisposable
    {
        private Action<Engine> EngineAction { get; }

        /// <summary>
        /// Список логов консиоли
        /// </summary>
        public List<JsLogggedVariables> ConsoleLogs { get; } = new List<JsLogggedVariables>();

        internal JsExecutorComponents Components { get; }
        public IServiceScope ServiceScope { get; }

        /// <summary>
        /// Системные логи времени выполнения
        /// </summary>
        public List<JsExecutionLog> ExecutionLogs { get; } = new List<JsExecutionLog>();

        /// <summary>
        /// Движок
        /// </summary>
        public Engine Engine { get; }
        public HandleJsCallWorker JsCallWorker { get; }

        internal JsExecutionContext(JsExecutorComponents components, 
            IServiceScope serviceScope,
            Action<Engine> engineAction)
        {
            JsCallWorker = new HandleJsCallWorker(components, serviceScope.ServiceProvider, this);
            Components = components;
            ServiceScope = serviceScope;
            EngineAction = engineAction;
            Engine = CreateEngine();
        }

        internal Engine CreateEngine()
        {
            var engine = new Engine()
                    .SetValue(JsConsts.InnerApiObjectName, JsCallWorker)
                    .SetValue("console", new
                    {
                        log = new Action<object[]>(Log)
                    });
            EngineAction?.Invoke(engine);

            engine.Execute(ScriptResources.ScriptInit);

            return engine;
        }

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

        public void Dispose()
        {
            ServiceScope.Dispose();
        }
    }
}