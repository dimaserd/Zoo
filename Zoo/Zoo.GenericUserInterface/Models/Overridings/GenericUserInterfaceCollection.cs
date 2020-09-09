using Croco.Core.Documentation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Переопределения пользовательских интерфейсов для типов
    /// </summary>
    public class GenericUserInterfaceCollection
    {
        readonly Dictionary<string, GenerateGenericUserInterfaceModel> ComputedInterfaces = new Dictionary<string, GenerateGenericUserInterfaceModel>();
        readonly Dictionary<Type, Overrider> InterfaceOverriders = new Dictionary<Type, Overrider>();
        readonly Dictionary<Type, DataProvider> DataProviders = new Dictionary<Type, DataProvider>();

        GenericInterfaceOptions Options { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options"></param>
        public GenericUserInterfaceCollection(GenericInterfaceOptions options = null)
        {
            Options = options ?? GenericInterfaceOptions.Default();
        }

        
        /// <summary>
        /// Добавить новое переопределение для интерфейса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overrider"></param>
        /// <returns></returns>
        public GenericUserInterfaceCollection AddOverrider<T>(GenericInterfaceOverrider<T> overrider) where T : class
        {
            var key = typeof(T);
            if (InterfaceOverriders.ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.OverridingForTypeIsAlreadySetFormat, key.FullName));
            }

            InterfaceOverriders.Add(key, new Overrider 
            {
                OverrideFunction = x => overrider.OverrideInterfaceAsync(new GenericUserInterfaceModelBuilder<T>(x)),
                Type = overrider.Type
            });

            return this;
        }

        internal GenericUserInterfaceCollection AddDataProviderForAutoCompletion<TDataProvider, TItem>(TDataProvider dataProvider) where TDataProvider: DataProviderForAutoCompletion<TItem>
        {
            var key = dataProvider.GetType();
            if (DataProviders.ContainsKey(key))
            {
                return this;
            }

            DataProviders.Add(key, new DataProvider
            {
                DataFunction = async x =>
                {
                    var data = await dataProvider.GetData(x);

                    return data.Select(x => x.ToAutoCompleteSuggestion()).ToArray();
                }
            });

            return this;
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

            var interfaceModel = new GenericUserInterfaceModelBuilder(type, Options).Result;

            var overriding = GetOverriding(type);

            if(overriding == null)
            {
                //Если переопределения нет, то интерфейс считается статическим
                ComputedInterfaces.Add(typeDisplayFullName, interfaceModel);
                return interfaceModel;
            }

            await overriding.OverrideFunction(interfaceModel);

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

            return InterfaceOverriders[key];
        }

        internal class Overrider
        {
            public Func<GenerateGenericUserInterfaceModel, Task> OverrideFunction { get; set; }
            public InterfaceOverriderType Type { get; set; }
        }

        internal class DataProvider
        {
            public Func<string, Task<AutoCompleteSuggestion[]>> DataFunction { get; set; }
        }
    }
}