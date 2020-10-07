using Croco.Core.Documentation.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models.Bag
{
    /// <summary>
    /// Портфель из пользовательских интерфейсов для типов и прочего добра необходимого для
    /// его построения
    /// </summary>
    public class GenericUserInterfaceBag
    {
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Какой тип нужно переопределить и каким переопределителем
        /// </summary>
        internal Dictionary<Type, Type> DefaultInterfaceOverriders { get; }
 
        internal Dictionary<string, Type> AutoCompletionDataProviders { get; }
        internal Dictionary<string, Type> SelectListDataProviders { get; }

        /// <summary>
        /// Посчитанные интерфейсы, ключ строка с названием типа данных
        /// </summary>
        readonly Dictionary<string, (GenerateGenericUserInterfaceModel, Type)> ComputedInterfaces = new Dictionary<string, (GenerateGenericUserInterfaceModel, Type)>();

        /// <summary>
        /// Опции для создания интерфейса
        /// </summary>
        public GenericInterfaceOptions Options { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="bagOptions"></param>
        /// <param name="options"></param>
        public GenericUserInterfaceBag(IServiceProvider serviceProvider, GenericUserInterfaceBagOptions bagOptions, GenericInterfaceOptions options)
        {
            SelectListDataProviders = bagOptions.SelectListDataProviders;
            DefaultInterfaceOverriders = bagOptions.DefaultInterfaceOverriders;
            AutoCompletionDataProviders = bagOptions.AutoCompletionDataProviders;
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
            if (!AutoCompletionDataProviders.ContainsKey(providerTypeFullName))
            {
                throw new InvalidOperationException("Провайдер данных не найден по полному названию типа");
            }

            var typeOfDataProvider = AutoCompletionDataProviders[providerTypeFullName];

            var provider = ServiceProvider.GetRequiredService(typeOfDataProvider) as IDataProviderForAutoCompletion;

            return provider.GetSuggestionsData(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerTypeFullName"></param>
        /// <returns></returns>
        public Task<SelectListItem[]> CallSelectListItemDataProvider(string providerTypeFullName)
        {
            if (!SelectListDataProviders.ContainsKey(providerTypeFullName))
            {
                throw new InvalidOperationException("Провайдер данных не найден по полному названию типа");
            }

            var typeOfDataProvider = SelectListDataProviders[providerTypeFullName];

            var provider = ServiceProvider.GetRequiredService(typeOfDataProvider) as IDataProviderForSelectList;

            return provider.GetSelectListItems();
        }

        /// <summary>
        /// Получить интерфейс
        /// </summary>
        /// <param name="typeDisplayFullName"></param>
        /// <returns></returns>
        public async Task<GenerateGenericUserInterfaceModel> GetDefaultInterface(string typeDisplayFullName)
        {
            var interfaceModelResult = await GetOrAddDefaultInterfaceFromComputed(typeDisplayFullName);

            var overriding = GetDefaultOverriding(interfaceModelResult.Item2);

            var interfaceModel = interfaceModelResult.Item1;
            if (overriding != null)
            {
                await overriding.SetDropDownDatasFunction(this, interfaceModel);
            }

            return interfaceModel;
        }

        /// <summary>
        /// Получить интерфейс
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public Task<GenerateGenericUserInterfaceModel> GetDefaultInterface<TModel>()
        {
            return GetDefaultInterface(typeof(TModel).FullName);
        }

        /// <summary>
        /// Вызывает все интерфейсы, которые были переопределены и если где-то произойдет ошибка, она будет выплюнута наружу.
        /// <para></para>
        /// Рекомендуется использовать в тестах для предотвращения ошибок в рантайме.
        /// </summary>
        /// <returns></returns>
        public async Task ValidateDefaultOverriders()
        {
            foreach (var overrider in DefaultInterfaceOverriders)
            {
                await GetDefaultInterface(overrider.Key.FullName);
            }
        }

        /// <summary>
        /// Получить переопрделение по полному названию типа
        /// </summary>
        /// <returns></returns>
        private Overrider GetDefaultOverriding(Type key)
        {
            if (!DefaultInterfaceOverriders.ContainsKey(key))
            {
                return null;
            }

            var overrider = ServiceProvider.GetRequiredService(DefaultInterfaceOverriders[key]) as IGenericInterfaceOverrider;

            return overrider.GetOverrider();
        }

        private async Task<(GenerateGenericUserInterfaceModel, Type)> GetOrAddDefaultInterfaceFromComputed(string typeDisplayFullName)
        {
            if (ComputedInterfaces.ContainsKey(typeDisplayFullName))
            {
                return ComputedInterfaces[typeDisplayFullName];
            }

            var type = CrocoTypeSearcher.FindFirstTypeByName(typeDisplayFullName, x => !x.IsGenericTypeDefinition);

            if (type == null)
            {
                return (null, null);
            }

            var builder = new GenericUserInterfaceModelBuilder(type, Options);

            var interfaceModel = builder.Result;

            var overriding = GetDefaultOverriding(type);

            if (overriding != null)
            {
                await overriding.MainOverrideFunction(this, interfaceModel);
                await ProcessClassesAndArrays(interfaceModel);
                ComputedInterfaces.Add(typeDisplayFullName, (interfaceModel, type));
            }

            await ProcessClassesAndArrays(interfaceModel);
            return (interfaceModel, type);
        }

        private async Task ProcessClassesAndArrays(GenerateGenericUserInterfaceModel interfaceModel)
        {
            foreach (var block in interfaceModel.Interface.Blocks)
            {
                if (block.InterfaceType == UserInterfaceType.GenericInterfaceForClass)
                {
                    var defaultInterface = await GetOrAddDefaultInterfaceFromComputed(block.TypeDisplayFullName);
                    block.InnerGenericInterface = defaultInterface.Item1.Interface;
                }
                else if(block.InterfaceType == UserInterfaceType.GenericInterfaceForArray)
                {
                    var defaultInterface = await GetOrAddDefaultInterfaceFromComputed(block.TypeDisplayFullName[0..^2]);
                    block.InnerGenericInterface = defaultInterface.Item1.Interface;
                }
            }
        }
    }
}