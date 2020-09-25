using System;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models.Bag;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Переопределитель интерфейсов
    /// </summary>
    public class Overrider
    {
        /// <summary>
        /// Функция для переопределения
        /// </summary>
        public Func<GenericUserInterfaceBag, GenerateGenericUserInterfaceModel, Task> OverrideFunction { get; set; }
    }
}