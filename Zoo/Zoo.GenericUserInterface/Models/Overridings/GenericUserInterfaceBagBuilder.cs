using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Abstractions;
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
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddOverrider<TOverrider>() where TOverrider : class, IGenericInterfaceOverrider
        {
            var type = typeof(TOverrider);

            AddOverriderInner(type, GetFirstInnerGeneric(type));
            ServiceCollection.AddTransient<TOverrider>();

            return this;
        }

        /// <summary>
        /// Добавить переопределение для интерфейса
        /// </summary>
        /// <typeparam name="TOverrider"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBagBuilder AddOverrider<TOverrider>(Func<IServiceProvider, TOverrider> providerFunc) where TOverrider : class, IGenericInterfaceOverrider
        {
            var type = typeof(TOverrider);
            
            AddOverriderInner(type, GetFirstInnerGeneric(type));
            ServiceCollection.AddTransient(providerFunc);

            return this;
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
    }
}