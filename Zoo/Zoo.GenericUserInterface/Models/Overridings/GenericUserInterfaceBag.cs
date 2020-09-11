using Croco.Core.Documentation.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models.Overridings
{

    /// <summary>
    /// Портфель из пользовательских интерфейсов для типов и прочего добра необходимого для
    /// его построения
    /// </summary>
    public class GenericUserInterfaceBag
    {
        IServiceProvider ServiceProvider { get; }

        Dictionary<Type, Type> InterfaceOverriders { get; }

        readonly Dictionary<string, GenerateGenericUserInterfaceModel> ComputedInterfaces = new Dictionary<string, GenerateGenericUserInterfaceModel>();
        readonly Dictionary<Type, DataProvider> ComputedDataProviders = new Dictionary<Type, DataProvider>();
        
        GenericInterfaceOptions Options { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="bagOptions"></param>
        /// <param name="options"></param>
        public GenericUserInterfaceBag(IServiceProvider serviceProvider, GenericUserInterfaceBagOptions bagOptions, GenericInterfaceOptions options)
        {
            InterfaceOverriders = bagOptions.InterfaceOverriders;
            ServiceProvider = serviceProvider;
            Options = options;
        }

        /// <summary>
        /// Получить провайдера данных
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public TDataProvider GetDataProvider<TDataProvider, TItem>() 
            where TDataProvider : IDataProviderForAutoCompletion<TItem>
        {
            var key = typeof(TDataProvider);
            if (ComputedDataProviders.ContainsKey(key))
            {
                throw new Exception("Провайдер данных не зарегистрирован");
            }

            return (TDataProvider)ServiceProvider.GetRequiredService(key);
        }

        /// <summary>
        /// Получить интерфейс
        /// </summary>
        /// <param name="typeDisplayFullName"></param>
        /// <returns></returns>
        public async Task<GenerateGenericUserInterfaceModel> GetInterface(string typeDisplayFullName)
        {
            var type = CrocoTypeSearcher.FindFirstTypeByName(typeDisplayFullName);
            
            if (type == null)
            {
                return null;
            }

            var builder = new GenericUserInterfaceModelBuilder(type, Options);

            var interfaceModel = builder.Result;

            var overriding = GetOverriding(type);

            if(overriding == null)
            {
                //Если переопределения нет, то интерфейс считается статическим
                ComputedInterfaces.Add(typeDisplayFullName, interfaceModel);
                return interfaceModel;
            }

            await overriding.OverrideFunction(this, interfaceModel);

            if (overriding.Type == InterfaceOverriderType.Static)
            {
                ComputedInterfaces.Add(typeDisplayFullName, interfaceModel);
            }

            return interfaceModel;
        }

        /// <summary>
        /// Получить интерфейс
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public Task<GenerateGenericUserInterfaceModel> GetInterface<TModel>()
        {
            return GetInterface(typeof(TModel).FullName);
        }

        /// <summary>
        /// Вызывает все интерфейсы, которые были переопределены и если где-то произойдет ошибка, она будет выплюнута наружу.
        /// <para></para>
        /// Рекомендуется использовать в тестах для предотвращения ошибок в рантайме.
        /// </summary>
        /// <returns></returns>
        public async Task ValidateOverriders()
        {
            foreach(var overrider in InterfaceOverriders)
            {
                await GetInterface(overrider.Key.FullName);
            }
        }

        /// <summary>
        /// Получить переопрделение по полному названию типа
        /// </summary>
        /// <returns></returns>
        private Overrider GetOverriding(Type key)
        {
            if (!InterfaceOverriders.ContainsKey(key))
            {
                return null;
            }

            var overrider = ServiceProvider.GetRequiredService(InterfaceOverriders[key]) as IGenericInterfaceOverrider;

            return overrider.GetOverrider();
        }
    }
}