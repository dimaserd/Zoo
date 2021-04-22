using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Models.Bag;
using Zoo.GenericUserInterface.Options;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Построитель портфеля обобщенных пользовательских интерфейсов
    /// </summary>
    public class GenericUserInterfaceBagBuilder
    {
        GenericInterfaceOptions InterfaceOptions { get; set; }

        string _applicationHostUrl;
        readonly Dictionary<Type, Type> DefaultInterfaceOverriders = new Dictionary<Type, Type>();
        readonly Dictionary<string, Type> AutoCompletionDataProviders = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> SelectListDataProviders = new Dictionary<string, Type>();

        IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceCollection"></param>
        public GenericUserInterfaceBagBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        /// <summary>
        /// Установить корневой url адрес
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder SetHostUrl(string hostUrl)
        {
            _applicationHostUrl = hostUrl;
            return this;
        }

        /// <summary>
        /// Добавить определение для интерфейса
        /// </summary>
        /// <typeparam name="TDefinition"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDefaultDefinition<TDefinition>() where TDefinition : class, IGenericInterfaceOverrider
        {
            return AddDefaultOverriderInner<TDefinition>(() => ServiceCollection.AddTransient<TDefinition>());
        }

        /// <summary>
        /// Добавить переопределение для интерфейса
        /// </summary>
        /// <typeparam name="TDefinition"></typeparam>
        /// <param name="providerFunc"></param>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDefaultDefinition<TDefinition>(Func<IServiceProvider, TDefinition> providerFunc) where TDefinition : class, IGenericInterfaceOverrider
        {
            return AddDefaultOverriderInner<TDefinition>(() => ServiceCollection.AddTransient(providerFunc));
        }

        /// <summary>
        /// Добавить провайдер данных для автокомплита
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDataProviderForAutoCompletion<TDataProvider>() where TDataProvider : class, IDataProviderForAutoCompletion
        {
            return AddDataProviderForAutoCompletionInner<TDataProvider>(() => ServiceCollection.AddTransient<TDataProvider>());
        }

        /// <summary>
        /// Добавить провайдер данных для автокомплита
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDataProviderForAutoCompletion<TDataProvider>(Func<IServiceProvider, TDataProvider> providerFunc) where TDataProvider : class, IDataProviderForAutoCompletion
        {
            return AddDataProviderForAutoCompletionInner<TDataProvider>(() => ServiceCollection.AddTransient(providerFunc));
        }

        /// <summary>
        /// Добавить провайдер данных для выпадающего списка
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDataProviderForSelectList<TDataProvider>() where TDataProvider : class, IDataProviderForSelectList
        {
            return AddDataProviderForSelectListInner<TDataProvider>(() => ServiceCollection.AddTransient<TDataProvider>());
        }

        /// <summary>
        /// Добавить провайдер данных для выпадающего списка
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <param name="providerFunc"></param>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDataProviderForSelectList<TDataProvider>(Func<IServiceProvider, TDataProvider> providerFunc) where TDataProvider : class, IDataProviderForSelectList
        {
            return AddDataProviderForSelectListInner<TDataProvider>(() => ServiceCollection.AddTransient(providerFunc));
        }


        /// <summary>
        /// Построить
        /// </summary>
        public void Build()
        {
            if(InterfaceOptions == null)
            {
                InterfaceOptions = GenericInterfaceOptions.Default();
            }

            ServiceCollection.AddSingleton(InterfaceOptions);
            ServiceCollection.AddSingleton(new GenericUserInterfaceBagOptions 
            {
                SelectListDataProviders = SelectListDataProviders,
                AutoCompletionDataProviders = AutoCompletionDataProviders,
                DefaultInterfaceOverriders = DefaultInterfaceOverriders,
                ApplicationHostUrl = _applicationHostUrl
            });
            ServiceCollection.AddSingleton<GenericUserInterfaceBag>();
        }

        private void AddDefaultOverriderRecord(Type overriderType, Type modelType)
        {
            if (DefaultInterfaceOverriders.ContainsKey(modelType))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.OverridingForTypeIsAlreadySetFormat, modelType.FullName));
            }

            DefaultInterfaceOverriders.Add(modelType, overriderType);
        }

        private Type GetFirstInnerGeneric(Type type)
        {
            var baseType = type.BaseType;

            if (!baseType.IsGenericType)
            {
                throw new InvalidOperationException("Базовый тип не является обобщенным");
            }

            return baseType.GenericTypeArguments
                .First();
        }

        private GenericUserInterfaceBagBuilder AddDefaultOverriderInner<TDefinition>(Action action) where TDefinition : class, IGenericInterfaceOverrider
        {
            var type = typeof(TDefinition);

            AddDefaultOverriderRecord(type, GetFirstInnerGeneric(type));
            action();

            return this;
        }

        private GenericUserInterfaceBagBuilder AddDataProviderForSelectListInner<TDataProvider>(Action action) where TDataProvider : class, IDataProviderForSelectList
        {
            var type = typeof(TDataProvider);
            SelectListDataProviders.Add(type.FullName, type);
            action();

            return this;
        }

        private GenericUserInterfaceBagBuilder AddDataProviderForAutoCompletionInner<TDataProvider>(Action action) where TDataProvider : class, IDataProviderForAutoCompletion
        {
            var type = typeof(TDataProvider);

            AutoCompletionDataProviders.Add(type.FullName, type);
            action();

            return this;
        }
    }
}