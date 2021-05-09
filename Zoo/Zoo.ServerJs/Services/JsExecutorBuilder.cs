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
        bool IsResultsStorageRegistered = false;
        bool IsHttpClientRegistered = false;
        bool IsPersistedStorageRegistered = false;

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
        /// <param name="srvProviderFunc"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddHttpClient(Func<IServiceProvider, ServerJsHttpClient> srvProviderFunc)
        {
            if (IsHttpClientRegistered)
            {
                throw new InvalidOperationException(ExceptionTexts.HttpClientProviderIsAlreadyRegistered);
            }

            ServiceCollection.AddSingleton<IServerJsHttpClient, ServerJsHttpClient>(srvProviderFunc);
            IsHttpClientRegistered = true;
            return this;
        }

        /// <summary>
        /// Добавить удаленное Апи
        /// </summary>
        /// <param name="remoteApi"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddRemoteApi(RemoteJsOpenApi remoteApi)
        {
            if (_remoteApis.ContainsKey(remoteApi.Name))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.RemoteApiWithNameAlreadyRegisteredFormat, remoteApi.Name));
            }

            _remoteApis.Add(remoteApi.Name, remoteApi);
            return this;
        }

        /// <summary>
        /// Регистрирует хранилище для персистентных данных как синглтон
        /// </summary>
        /// <typeparam name="TStorage"></typeparam>
        /// <returns></returns>
        public JsExecutorBuilder AddPersistedStorage<TStorage>() where TStorage : class, IPersistedStorage
        {
            if (IsPersistedStorageRegistered)
            {
                throw new InvalidOperationException(ExceptionTexts.PersistedStorageIsAlreadyRegistered);
            }

            ServiceCollection.AddSingleton<IPersistedStorage, TStorage>();
            IsPersistedStorageRegistered = true;
            return this;
        }

        /// <summary>
        /// Регистрирует хранилище для персистентных данных как синглтон
        /// </summary>
        /// <typeparam name="TStorage"></typeparam>
        /// <param name="providerFunc"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddPersistedStorage<TStorage>(Func<IServiceProvider, TStorage> providerFunc) where TStorage : class, IPersistedStorage
        {
            if (IsPersistedStorageRegistered)
            {
                throw new InvalidOperationException(ExceptionTexts.PersistedStorageIsAlreadyRegistered);
            }

            ServiceCollection.AddSingleton<IPersistedStorage, TStorage>(providerFunc);
            IsPersistedStorageRegistered = true;
            return this;
        }

        /// <summary>
        /// Регистрирует хранилище для скриптов как синглтон
        /// </summary>
        /// <typeparam name="TStorage"></typeparam>
        /// <returns></returns>
        public JsExecutorBuilder AddScriptStorage<TStorage>() where TStorage : class, IJsScriptTaskStorage
        {
            if (IsResultsStorageRegistered)
            {
                throw new InvalidOperationException(ExceptionTexts.ScriptStorageIsAlreadyRegistered);
            }

            ServiceCollection.AddSingleton<IJsScriptTaskStorage, TStorage>();
            IsResultsStorageRegistered = true;
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
        /// Добавить нового рабочего
        /// </summary>
        /// <param name="jsWorker"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddJsWorker(IJsWorker jsWorker)
        {
            return AddJsWorker(jsWorker.JsWorkerDocs);
        }

        /// <summary>
        /// Зарегистрировать класс <see cref="JsExecutor"/> в контейнере зависимостей
        /// </summary>
        /// <param name="engineAction"></param>
        /// <param name="scopedServiceProviderAction"></param>
        public void Build(Action<Engine> engineAction = null, Action<IServiceProvider> scopedServiceProviderAction = null)
        {
            ServiceCollection.AddSingleton(new JsExecutorProperties
            {
                EngineAction = engineAction,
                ScopedServiceProviderAction = scopedServiceProviderAction,
                ExternalComponents = _components,
                JsWorkers = _jsWorkers,
                RemoteApis = _remoteApis
            });
            ServiceCollection.AddSingleton<JsExecutor>();
            
            if (!IsResultsStorageRegistered)
            {
                ServiceCollection.AddSingleton<IJsScriptTaskStorage, DefaultJsScriptResultStorage>();
            }

            if (!IsHttpClientRegistered)
            {
                AddHttpClient(srv => new ServerJsHttpClient(new HttpClient()));
            }

            if (!IsPersistedStorageRegistered)
            {
                ServiceCollection.AddSingleton<IPersistedStorage, FilePersistedStorage>();
            }
        }
    }
}