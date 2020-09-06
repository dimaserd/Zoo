using Croco.Core.Documentation.Services;
using System;
using System.Collections.Generic;
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
        readonly Dictionary<Type, Overrider> Dictionary = new Dictionary<Type, Overrider>();

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
            if (Dictionary.ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.OverridingForTypeIsAlreadySetFormat, key.FullName));
            }

            Dictionary.Add(key, new Overrider 
            {
                OverrideFunction = x => overrider.OverrideInterfaceAsync(new GenericUserInterfaceModelBuilder<T>(x)),
                Type = overrider.Type
            });

            return this;
        }

        /// <summary>
        /// Получить интерфейс
        /// </summary>
        /// <param name="typeDisplayFullName"></param>
        /// <returns></returns>
        public Task<GenerateGenericUserInterfaceModel> GetInterface(string typeDisplayFullName)
        {
            return ComputedInterfaces.GetOrAddAsync(typeDisplayFullName, async () => 
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
            });
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
        /// Получить переопрделение по полному названию типа
        /// </summary>
        /// <returns></returns>
        private Overrider GetOverriding(Type key)
        {
            if (!Dictionary.ContainsKey(key))
            {
                return null;
            }

            return Dictionary[key];
        }

        internal class Overrider
        {
            public Func<GenerateGenericUserInterfaceModel, Task> OverrideFunction { get; set; }
            public InterfaceOverriderType Type { get; set; }
        }
    }
}