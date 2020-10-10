using Jint;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
        readonly Dictionary<string, ExternalJsComponent> _components = new Dictionary<string, ExternalJsComponent>();
        readonly Dictionary<string, JsWorkerDocumentation> _jsWorkers = new Dictionary<string, JsWorkerDocumentation>();
        readonly Dictionary<string, RemoteJsOpenApi> _remoteApis = new Dictionary<string, RemoteJsOpenApi>();
        Func<IServiceProvider, HttpClient> _httpClientFactory;

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
        /// Добавить фабрику Http клиентов для вызовов удаленных сервисов
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddHttpClientFactory(Func<IServiceProvider, HttpClient> httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            return this;
        }

        /// <summary>
        /// Добавить удаленное Апи
        /// </summary>
        /// <param name="remoteApi"></param>
        /// <returns></returns>
        public JsExecutorBuilder AddRemoteApi(RemoteJsOpenApi remoteApi)
        {
            if(_httpClientFactory == null)
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
                RemoteApis = _remoteApis,
                HttpClientProvider = _httpClientFactory ?? throw new InvalidOperationException("необходимо предоставить фабрику Http клиентов")
            });
            ServiceCollection.AddSingleton<JsExecutor>();
        }
    }
}