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
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddOverrider<TOverrider>(Func<IServiceProvider, TOverrider> providerFunc) where TOverrider : class, IGenericInterfaceOverrider
        {
            return AddOverriderInner<TOverrider>(() => ServiceCollection.AddTransient(providerFunc));
        }

        /// <summary>
        /// Добавить провайдер данных для автокомплита
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDataProviderForAutoCompletion<TDataProvider, TItem>() where TDataProvider : DataProviderForAutoCompletion<TItem>
        {
            return AddDataProviderForAutoCompletionInner<TDataProvider, TItem>(() => ServiceCollection.AddTransient<TDataProvider>());
        }

        /// <summary>
        /// Добавить провайдер данных для автокомплита
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDataProviderForAutoCompletion<TDataProvider, TItem>(Func<IServiceProvider, TDataProvider> providerFunc) where TDataProvider : DataProviderForAutoCompletion<TItem>
        {
            return AddDataProviderForAutoCompletionInner<TDataProvider, TItem>(() => ServiceCollection.AddTransient(providerFunc));
        }

        /// <summary>
        /// Добавить провайдер данных для выпадающего списка
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDataProviderForSelectList<TDataProvider, TItem>() where TDataProvider : DataProviderForSelectList<TItem>
        {
            return AddDataProviderForSelectListInner<TDataProvider, TItem>(() => ServiceCollection.AddTransient<TDataProvider>());
        }

        /// <summary>
        /// Добавить провайдер данных для выпадающего списка
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="providerFunc"></param>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDataProviderForSelectList<TDataProvider, TItem>(Func<IServiceProvider, TDataProvider> providerFunc) where TDataProvider : DataProviderForSelectList<TItem>
        {
            return AddDataProviderForSelectListInner<TDataProvider, TItem>(() => ServiceCollection.AddTransient(providerFunc));
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

        private GenericUserInterfaceBagBuilder AddDataProviderForSelectListInner<TDataProvider, TItem>(Action action) where TDataProvider : DataProviderForSelectList<TItem>
        {
            var type = typeof(TDataProvider);
            SelectListDataProviders.Add(type.FullName, type);
            action();

            return this;
        }

        private GenericUserInterfaceBagBuilder AddDataProviderForAutoCompletionInner<TDataProvider, TItem>(Action action) where TDataProvider : DataProviderForAutoCompletion<TItem>
        {
            var type = typeof(TDataProvider);
            AutoCompletionDataProviders.Add(type.FullName, type);
            action();

            return this;
        }

    }
}