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
        Dictionary<string, Type> DataProviders { get; }

        readonly Dictionary<string, GenerateGenericUserInterfaceModel> ComputedInterfaces = new Dictionary<string, GenerateGenericUserInterfaceModel>();
        
        public GenericInterfaceOptions Options { get; }

        public static GenericUserInterfaceBag CreateDefault()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            return new GenericUserInterfaceBag(serviceProvider, new GenericUserInterfaceBagOptions 
            {
                DataProviders = new Dictionary<string, Type>(),
                InterfaceOverriders = new Dictionary<Type, Type>()
            }, GenericInterfaceOptions.Default());
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="bagOptions"></param>
        /// <param name="options"></param>
        public GenericUserInterfaceBag(IServiceProvider serviceProvider, GenericUserInterfaceBagOptions bagOptions, GenericInterfaceOptions options)
        {
            InterfaceOverriders = bagOptions.InterfaceOverriders;
            DataProviders = bagOptions.DataProviders;
            ServiceProvider = serviceProvider;
            Options = options;
        }

        /// <summary>
        /// Получить результат выполнения функции с данными по имени провайдера
        /// </summary>
        /// <param name="input"></param>
        /// <param name="providerTypeFullName"></param>
        /// <returns></returns>
        public Task<AutoCompleteSuggestion[]> CallAutoCompleteDataProvider(string input, string providerTypeFullName)
        {
            if (DataProviders.ContainsKey(providerTypeFullName))
            {
                throw new Exception("Провайдер данных не найден по полному названию типа");
            }

            var typeOfDataProvider = DataProviders[providerTypeFullName];

            var provider = ServiceProvider.GetRequiredService(typeOfDataProvider) as IDataProviderForAutoCompletion;

            return provider.GetSuggestionsData(input);
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