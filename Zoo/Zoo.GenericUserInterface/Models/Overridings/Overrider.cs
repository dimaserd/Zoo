using System;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Переопределитель интерфейсов
    /// </summary>
    public class Overrider
    {
        /// <summary>
        /// Основная Функция для переопределения
        /// </summary>
        public Func<GenericUserInterfaceBag, GenerateGenericUserInterfaceModel, Task> MainOverrideFunction { get; set; }

        /// <summary>
        /// Вторая функция для переопределения
        /// </summary>
        public Func<GenericUserInterfaceBag, GenerateGenericUserInterfaceModel, Task> SetDropDownDatasFunction { get; set; }
    }
}