using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Abstractions
{
    /// <summary>
    /// Построитель для блока
    /// </summary>
    public interface IGenericInterfaceBlockBuilder : IGenericInterfaceBuilder
    {
        /// <summary>
        /// Редактируемый блок
        /// </summary>
        UserInterfaceBlock Block { get; }
    }

    /// <summary>
    /// Абстракция построителя интерфейсов
    /// </summary>
    public interface IGenericInterfaceBuilder
    {
        /// <summary>
        /// Результат - модель для построения пользовательского интерфейса
        /// </summary>
        GenerateGenericUserInterfaceModel Result { get; }

        /// <summary>
        /// Портфель из логики переопределений интерфейсов
        /// </summary>
        GenericUserInterfaceBag Bag { get; }
    }
}