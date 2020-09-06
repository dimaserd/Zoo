using System.Threading.Tasks;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Переопределитель интерфейса
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericInterfaceOverrider<T> where T : class
    {
        internal InterfaceOverriderType Type => InterfaceOverriderType.Computed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="overrider"></param>
        /// <returns></returns>
        public abstract Task OverrideInterfaceAsync(GenericUserInterfaceModelBuilder<T> overrider);
    }
}