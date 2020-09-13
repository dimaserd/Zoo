using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Overridings;

namespace Zoo.GenericUserInterface.Abstractions
{
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