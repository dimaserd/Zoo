using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
        readonly Dictionary<string, Type> DataProviders = new Dictionary<string, Type>();
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
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddOverrider<TOverrider, TModel>() 
            where TOverrider : GenericInterfaceOverrider<TModel>
            where TModel : class
        {
            AddOverriderInner<TOverrider, TModel>();
            ServiceCollection.AddTransient<TOverrider>();
            return this;
        }

        /// <summary>
        /// Добавить переопределение для интерфейса
        /// </summary>
        /// <typeparam name="TOverrider"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddOverrider<TOverrider, TModel>(Func<IServiceProvider, TOverrider> providerFunc)
            where TOverrider : GenericInterfaceOverrider<TModel>
            where TModel : class
        {
            AddOverriderInner<TOverrider, TModel>();
            ServiceCollection.AddTransient(providerFunc);
            return this;
        }

        private void AddOverriderInner<TOverrider, TModel>()
            where TOverrider : GenericInterfaceOverrider<TModel>
            where TModel : class
        {
            var key = typeof(TModel);
            if (InterfaceOverriders.ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.OverridingForTypeIsAlreadySetFormat, key.FullName));
            }

            InterfaceOverriders.Add(key, typeof(TOverrider));
        }

        /// <summary>
        /// Добавить провайдер данных
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddDataProvider<TDataProvider, TItem>() where TDataProvider : DataProviderForAutoCompletion<TItem>
        {
            var type = typeof(TDataProvider);
            DataProviders.Add(type.FullName, type);
            ServiceCollection.AddTransient<TDataProvider>();
            
            return this;
        }

        internal bool HasDataProvider<TDataProvider, TItem>()
        {
            return DataProviders.ContainsKey(typeof(TDataProvider).FullName);
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
                DataProviders = DataProviders,
                InterfaceOverriders = InterfaceOverriders
            });
            ServiceCollection.AddSingleton<GenericUserInterfaceBag>();
        }
    }
}