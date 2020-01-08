using System;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Реализация 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericInterfaceOverriderImplementation<T> : GenericInterfaceOverrider<T> where T : class
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="task"></param>
        public GenericInterfaceOverriderImplementation(Func<GenericUserInterfaceModelBuilder<T>, Task> task)
        {
            Task = task;
        }

        Func<GenericUserInterfaceModelBuilder<T>, Task> Task { get; }

        /// <summary>
        /// Переопределение 
        /// </summary>
        /// <param name="overrider"></param>
        /// <returns></returns>
        public override Task OverrideInterfaceAsync(GenericUserInterfaceModelBuilder<T> overrider)
        {
            return Task(overrider);
        }
    }
}