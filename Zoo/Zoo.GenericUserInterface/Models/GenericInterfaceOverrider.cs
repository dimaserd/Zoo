using System;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Переопределитель обобщенного интерфейса
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericInterfaceOverrider<T> where T : class
    {
        /// <summary>
        /// Полное название типа
        /// </summary>
        public string TypeFullName { get; } = typeof(T).FullName;

        /// <summary>
        /// Переорпеделить интерфейс
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract Task OverrideInterfaceAsync(GenericUserInterfaceModelBuilder<T> model);
    }

    internal class GenericInterfaceOverriderImplementation<T> : GenericInterfaceOverrider<T> where T : class
    {
        internal GenericInterfaceOverriderImplementation(Func<GenericUserInterfaceModelBuilder<T>, Task> overrideTask)
        {
            OverrideTask = overrideTask;
        }

        public Func<GenericUserInterfaceModelBuilder<T>, Task> OverrideTask { get; }

        public override Task OverrideInterfaceAsync(GenericUserInterfaceModelBuilder<T> model)
        {
            return OverrideTask(model);
        }
    }

    public abstract class GenericInterfaceSelfOverrider<T> where T : class, new()
    {
        /// <summary>
        /// Полное название типа
        /// </summary>
        public string TypeFullName { get; } = typeof(T).FullName;

        /// <summary>
        /// Получить переопределитель интерфейса
        /// </summary>
        /// <returns></returns>
        public GenericInterfaceOverrider<T> GetOverrider()
        {
            return new GenericInterfaceOverriderImplementation<T>(SelfOverrideInterfaceAsync);
        }

        /// <summary>
        /// Переопределить интерфейс
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract Task SelfOverrideInterfaceAsync(GenericUserInterfaceModelBuilder<T> model);
    }
}