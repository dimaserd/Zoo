using Zoo.GenericUserInterface.Models.Overridings;

namespace Zoo.GenericUserInterface.Abstractions
{
    /// <summary>
    /// Перепоределитель интерфейса
    /// </summary>
    public interface IGenericInterfaceOverrider
    {
        /// <summary>
        /// Получить переопределитель
        /// </summary>
        /// <returns></returns>
        Overrider GetOverrider();
    }
}