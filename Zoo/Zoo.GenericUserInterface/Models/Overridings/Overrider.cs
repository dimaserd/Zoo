using System;
using System.Threading.Tasks;

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

        /// <summary>
        /// Тип высчитывания переопределителя интерфейса
        /// </summary>
        public InterfaceOverriderType Type { get; set; }
    }
}