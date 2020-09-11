using Zoo.GenericUserInterface.Models;

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
    }
}