using Jint;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Resources;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Построитель для <see cref="JsExecutor"/>
    /// </summary>
    public class JsExecutorBuilder
    {
        bool IsStorageRegistered = false;
        bool IsHttpClientProviderRegistered = false;

        readonly Dictionary<string, ExternalJsComponent> _components = new Dictionary<string, ExternalJsComponent>();
        readonly Dictionary<string, JsWorkerDocumentation> _jsWorkers = new Dictionary<string, JsWorkerDocumentation>();
        readonly Dictionary<string, RemoteJsOpenApi> _remoteApis = new Dictionary<string, RemoteJsOpenApi>();
        
        internal IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceCollection"></param>
        public JsExecutorBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        /// <summary>
        /// Добавить фабрику Http клиентов для вызовов удаленных сервисов.
        /// Регистрирует данный объект как синглтон.
        /// </summary>
        /// <returns></returns>
        public JsExecutorBuilder AddHttpClientFactory<THttpClientProvider>() where THttpClientProvider : class, IServerJsHttpClientProvider
        {
            if (IsHttpClientProviderRegistered)
            {
                throw new InvalidOperationException(ExceptionTexts.HttpClientProviderIsAlreadyRegistered);
            }

            ServiceCollection.AddSingleton<IServerJsHttpClientProvider, THttpClientProvider>();
            IsHttpClientProviderRegistered = true;
            return this;
        }

        /// <summary>
        /// Добавить удаленное Апи
        /// </summary>
        /// <param name="remoteApi"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddRemoteApi(RemoteJsOpenApi remoteApi)
        {
            if(!IsHttpClientProviderRegistered)
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.NeedSettingHttpClientBeforeUseMethodFormat, nameof(AddHttpClientFactory)));
            }

            if (_remoteApis.ContainsKey(remoteApi.Name))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.RemoteApiWithNameAlreadyRegisteredFormat, remoteApi.Name));
            }

            _remoteApis.Add(remoteApi.Name, remoteApi);
            return this;
        }

        /// <summary>
        /// Регистрирует хранилище для скриптов как синглтон
        /// </summary>
        /// <typeparam name="TStorage"></typeparam>
        /// <returns></returns>
        public JsExecutorBuilder AddScriptStorage<TStorage>() where TStorage : class, IJsScriptResultStorage
        {
            if (IsStorageRegistered)
            {
                throw new InvalidOperationException(ExceptionTexts.ScriptStorageIsAlreadyRegistered);
            }

            ServiceCollection.AddSingleton<IJsScriptResultStorage, TStorage>();
            IsStorageRegistered = true;
            return this;
        }

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
                throw new Exception(string.Format(ExceptionTexts.ComponentWithNameAlreadyRegisteredFormat, component.ComponentName));
            }

            _components.Add(component.ComponentName, component);
            return this;
        }

        /// <summary>
        /// Добавить нового рабочего
        /// </summary>
        /// <param name="jsWorkerBuilderFunc"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddJsWorker(Func<JsWorkerBuilder, JsWorkerDocumentation> jsWorkerBuilderFunc)
        {
            var jsWorkerBuilder = new JsWorkerBuilder(this);

            var jsWorker = jsWorkerBuilderFunc(jsWorkerBuilder);

            jsWorker.Validate();
            if (_jsWorkers.ContainsKey(jsWorker.WorkerName))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.JsWorkerWithNameAlreadyRegisteredFormat, jsWorker.WorkerName));
            }

            _jsWorkers.Add(jsWorker.WorkerName, jsWorker);
            return this;
        }

        /// <summary>
        /// Зарегистрировать класс <see cref="JsExecutor"/> в контейнере зависимостей
        /// </summary>
        /// <param name="action"></param>
        public void Build(Action<Engine> action = null)
        {
            ServiceCollection.AddSingleton(new JsExecutorProperties
            {
                EngineAction = action,
                ExternalComponents = _components,
                JsWorkers = _jsWorkers,
                RemoteApis = _remoteApis
            });
            ServiceCollection.AddSingleton<JsExecutor>();
            
            if (!IsStorageRegistered)
            {
                ServiceCollection.AddSingleton<IJsScriptResultStorage, DefaultJsScriptResultStorage>();
            }
        }
    }
}