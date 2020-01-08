using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Переопределения пользовательских интерфейсов для типов
    /// </summary>
    public class GenericUserInterfaceOverridings
    {
        readonly Dictionary<string, Func<GenerateGenericUserInterfaceModel, Task>> Dictionary = new Dictionary<string, Func<GenerateGenericUserInterfaceModel, Task>>();

        /// <summary>
        /// Добавить новое переопределение для интерфейса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overrider"></param>
        /// <returns></returns>
        public GenericUserInterfaceOverridings Add<T>(GenericInterfaceOverrider<T> overrider) where T : class
        {
            if (Dictionary.ContainsKey(overrider.TypeFullName))
            {
                throw new ApplicationException($"Переопределение для типа {overrider.TypeFullName} уже задано");
            }

            Dictionary.Add(overrider.TypeFullName, x => overrider.OverrideInterfaceAsync(new Services.GenericUserInterfaceModelBuilder<T>(x)));

            return this;
        }

        /// <summary>
        /// Получить переопрделение по полному названию типа
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public Func<GenerateGenericUserInterfaceModel, Task> GetOverriding(string typeFullName)
        {
            if (!Dictionary.ContainsKey(typeFullName))
            {
                return null;
            }

            return Dictionary[typeFullName];
        }
    }
}