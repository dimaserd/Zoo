using System;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Services.Definition
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