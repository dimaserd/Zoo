using Jint;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Resources;
using Zoo.ServerJs.Services.Internal;
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
            Action<Engine> engineAction,
            ILogger logger,
            Action<IServiceProvider> scopedServiceProviderAction)
        {
            var serviceProvider = serviceScope.ServiceProvider;
            scopedServiceProviderAction?.Invoke(serviceProvider);

            JsCallWorker = new HandleJsCallWorker(components, serviceProvider, this, logger);
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
                SerializedVariables = objs?.Select(ToJsSerializedVariable).ToList() ?? new List<JsSerializedVariable>()
            });
        }

        private JsSerializedVariable ToJsSerializedVariable(object obj)
        {
            if(obj == null)
            {
                return new JsSerializedVariable
                {
                    DataJson = "null",
                    TypeFullName = null
                };
            }

            return new JsSerializedVariable
            {
                DataJson = ZooSerializer.Serialize(obj),
                TypeFullName = obj.GetType().FullName
            };
        }

        public void Dispose()
        {
            ServiceScope.Dispose();
        }
    }
}