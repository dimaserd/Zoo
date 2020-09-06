using Jint;
using System;
using System.Collections.Generic;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Построитель для <see cref="JsExecutor"/>
    /// </summary>
    public class JsExecutorBuilder
    {
        readonly Dictionary<string, ExternalJsComponent> _components = new Dictionary<string, ExternalJsComponent>();
        readonly Dictionary<string, IJsWorker> _workers = new Dictionary<string, IJsWorker>();

        /// <summary>
        /// Добавить внешние компоненты
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddExternalComponents(IEnumerable<ExternalJsComponent> components)
        {
            foreach(var component in components)
            {
                AddExternalComponent(component);
            }
            return this;
        }

        /// <summary>
        /// Добавить внешний компонент
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddExternalComponent(ExternalJsComponent component)
        {
            if(_components.ContainsKey(component.ComponentName))
            {
                throw new Exception($"Компонент с названием '{component.ComponentName}' уже зарегистрирован");
            }

            _components.Add(component.ComponentName, component);
            return this;
        }

        /// <summary>
        /// Добавить нового рабочего
        /// </summary>
        /// <param name="jsWorker"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddJsWorker(IJsWorker jsWorker)
        {
            var docs = jsWorker.JsWorkerDocs();
            if (_workers.ContainsKey(docs.WorkerName))
            {
                throw new Exception($"Рабочий класс с названием '{docs.WorkerName}' уже зарегистрирован");
            }

            _workers.Add(docs.WorkerName, jsWorker);
            return this;
        }

        /// <summary>
        /// Добавить новых рабочих
        /// </summary>
        /// <param name="jsWorkers"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddJsWorkers(IEnumerable<IJsWorker> jsWorkers)
        {
            foreach(var jsWorker in jsWorkers)
            {
                AddJsWorker(jsWorker);
            }
            return this;
        }

        /// <summary>
        /// Построить исполнителя скриптов
        /// </summary>
        /// <returns></returns>
        public JsExecutor BuildJsExecutor(Action<Engine> action = null)
        {
            return new JsExecutor(new JsExecutorProperties
            {
                EngineAction = action,
                ExternalComponents = _components,
                JsWorkers = _workers
            }); ;
        }
    }
}