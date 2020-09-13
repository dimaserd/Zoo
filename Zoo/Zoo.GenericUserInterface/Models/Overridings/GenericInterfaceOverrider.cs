using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Переопределитель интерфейса
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericInterfaceOverrider<T> : IGenericInterfaceOverrider
        where T : class 
    {
        internal InterfaceOverriderType InterfaceType => InterfaceOverriderType.Computed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bag"></param>
        /// <param name="overrider"></param>
        /// <returns></returns>
        public abstract Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<T> overrider);

        /// <summary>
        /// Получить переопределитель
        /// </summary>
        /// <returns></returns>
        public Overrider GetOverrider()
        {
            return new Overrider
            {
                Type = InterfaceType,
                OverrideFunction = (bag, model) =>
                {
                    var builder = new GenericUserInterfaceModelBuilder<T>(model, bag);

                    return OverrideInterfaceAsync(bag, builder);
                }
            };
        }
    }
}