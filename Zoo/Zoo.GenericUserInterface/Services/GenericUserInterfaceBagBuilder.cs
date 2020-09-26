using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Models.Bag;
using Zoo.GenericUserInterface.Models.Providers;
using Zoo.GenericUserInterface.Resources;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Построитель портфеля обобщенных пользовательских интерфейсов
    /// </summary>
    public class GenericUserInterfaceBagBuilder
    {
        GenericInterfaceOptions InterfaceOptions { get; set; }
        readonly Dictionary<Type, Type> InterfaceOverriders = new Dictionary<Type, Type>();
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
        /// Добавить переопределение для интерфейса
        /// </summary>
        /// <typeparam name="TOverrider"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddOverrider<TOverrider>() where TOverrider : class, IGenericInterfaceOverrider
        {
            return AddOverriderInner<TOverrider>(() => ServiceCollection.AddTransient<TOverrider>());
        }

        /// <summary>
        /// Добавить переопределение для интерфейса
        /// </summary>
        /// <typeparam name="TOverrider"></typeparam>
        /// <param name="providerFunc"></param>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddOverrider<TOverrider>(Func<IServiceProvider, TOverrider> providerFunc) where TOverrider : class, IGenericInterfaceOverrider
        {
            return AddOverriderInner<TOverrider>(() => ServiceCollection.AddTransient(providerFunc));
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
                InterfaceOverriders = InterfaceOverriders
            });
            ServiceCollection.AddSingleton<GenericUserInterfaceBag>();
        }

        private void AddOverriderInner(Type overriderType, Type modelType)
        {
            if (InterfaceOverriders.ContainsKey(modelType))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.OverridingForTypeIsAlreadySetFormat, modelType.FullName));
            }

            InterfaceOverriders.Add(modelType, overriderType);
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

        private GenericUserInterfaceBagBuilder AddOverriderInner<TOverrider>(Action action) where TOverrider : class, IGenericInterfaceOverrider
        {
            var type = typeof(TOverrider);

            AddOverriderInner(type, GetFirstInnerGeneric(type));
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