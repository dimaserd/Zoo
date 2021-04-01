using Croco.Core.Documentation.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Bag;
using Zoo.GenericUserInterface.Models.Definition;
using Zoo.GenericUserInterface.Resources;

namespace Zoo.GenericUserInterface.Services
{
    /// <summary>
    /// Портфель из пользовательских интерфейсов для типов и прочего добра необходимого для
    /// его построения
    /// </summary>
    public class GenericUserInterfaceBag
    {
        IServiceProvider ServiceProvider { get; }

        string ApplicationHostUrl { get; }

        /// <summary>
        /// Какой тип нужно переопределить и каким переопределителем
        /// </summary>
        internal ConcurrentDictionary<Type, Type> DefaultInterfaceOverriders { get; }

        internal ConcurrentDictionary<string, Type> AutoCompletionDataProviders { get; }
        internal ConcurrentDictionary<string, Type> SelectListDataProviders { get; }

        internal ConcurrentDictionary<string, Type> TypeSearchMatchings = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Посчитанные интерфейсы, ключ строка с названием типа данных
        /// </summary>
        readonly ConcurrentDictionary<Type, GenerateGenericUserInterfaceModel> ComputedInterfaces = new ConcurrentDictionary<Type, GenerateGenericUserInterfaceModel>();

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
            ApplicationHostUrl = bagOptions.ApplicationHostUrl;
            SelectListDataProviders = new ConcurrentDictionary<string, Type>(bagOptions.SelectListDataProviders);
            DefaultInterfaceOverriders = new ConcurrentDictionary<Type, Type>(bagOptions.DefaultInterfaceOverriders);
            AutoCompletionDataProviders = new ConcurrentDictionary<string, Type>(bagOptions.AutoCompletionDataProviders);
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
            var interfaceModelWithType = await GetOrAddDefaultInterfaceFromComputed(typeDisplayFullName);

            if(interfaceModelWithType == null)
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.TypeNotFoundByThisNameFormat, typeDisplayFullName));
            }

            var overriding = GetDefaultOverriding(interfaceModelWithType.Type);

            var interfaceModel = interfaceModelWithType.InterfaceModel;
            if (overriding != null)
            {
                await overriding.SetDropDownDatasFunction(this, interfaceModel);
            }

            return interfaceModel;
        }

        /// <summary>
        /// Получить модель для генерации пользовательского интерфейса на удаленной машине
        /// </summary>
        /// <param name="typeDisplayFullName"></param>
        /// <returns></returns>
        public async Task<GenerateOnRemoteUserInterfaceModel> GetDefaultInterfaceOnRemote(string typeDisplayFullName)
        {
            var interfaceModel = await GetDefaultInterface(typeDisplayFullName);

            return new GenerateOnRemoteUserInterfaceModel
            {
                ApplicationHostUrl = ApplicationHostUrl,
                GemerateInterfaceModel = interfaceModel
            };
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

        private Type SearchTypeFromCache(string typeDisplayFullName)
        {
            if (TypeSearchMatchings.ContainsKey(typeDisplayFullName))
            {
                return TypeSearchMatchings[typeDisplayFullName];
            }

            var type = CrocoTypeSearcher.FindFirstTypeByName(typeDisplayFullName, x => !x.IsGenericTypeDefinition);

            if (type != null)
            {
                TypeSearchMatchings[typeDisplayFullName] = type;
            }

            return type;
        }

        private async Task<TypeWithInterfaceModel> GetOrAddDefaultInterfaceFromComputed(string typeDisplayFullName)
        {
            var type = SearchTypeFromCache(typeDisplayFullName);

            if (type == null)
            {
                return null;
            }

            if (ComputedInterfaces.ContainsKey(type))
            {
                return new TypeWithInterfaceModel 
                {
                    Type = type,
                    InterfaceModel = ComputedInterfaces[type]
                };
            }

            var builder = new GenericUserInterfaceModelBuilder(type, Options);

            var interfaceModel = builder.Result;

            var overriding = GetDefaultOverriding(type);

            if (overriding != null)
            {
                await overriding.MainOverrideFunction(this, interfaceModel);
                await ProcessClassesAndArrays(interfaceModel);
                ComputedInterfaces.TryAdd(type, interfaceModel);
            }

            await ProcessClassesAndArrays(interfaceModel);

            return new TypeWithInterfaceModel
            {
                Type = type,
                InterfaceModel = interfaceModel
            };
        }

        private async Task ProcessClassesAndArrays(GenerateGenericUserInterfaceModel interfaceModel)
        {
            foreach (var block in interfaceModel.Interface.Blocks)
            {
                if (block.InterfaceType == UserInterfaceType.GenericInterfaceForClass)
                {
                    var defaultInterface = await GetOrAddDefaultInterfaceFromComputed(block.TypeDisplayFullName);

                    if(defaultInterface == null)
                    {
                        throw new NotImplementedException($"Не могу создать интерфейс для {block.TypeDisplayFullName}");
                    }
                    block.InnerGenericInterface = defaultInterface.InterfaceModel.Interface;
                }
                else if (block.InterfaceType == UserInterfaceType.GenericInterfaceForArray)
                {
                    var defaultInterface = await GetOrAddDefaultInterfaceFromComputed(block.TypeDisplayFullName[0..^2]);
                    block.InnerGenericInterface = defaultInterface.InterfaceModel.Interface;
                }
            }
        }

        internal class TypeWithInterfaceModel
        {
            public Type Type { get; set; }
            public GenerateGenericUserInterfaceModel InterfaceModel { get; set; }
        }
    }
}