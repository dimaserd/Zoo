using System.Threading.Tasks;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Переопределитель интерфейса
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericInterfaceOverrider<T> where T : class
    {
        /// <summary>
        /// Полное название типа
        /// </summary>
        public string TypeFullName => typeof(T).FullName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="overrider"></param>
        /// <returns></returns>
        public abstract Task OverrideInterfaceAsync(GenericUserInterfaceModelBuilder<T> overrider);
    }
}