namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Тип высчитывания переопределителя интерфейса
    /// </summary>
    public enum InterfaceOverriderType
    {
        /// <summary>
        /// Переопределение для интерфейса, высчитывается каждый раз
        /// </summary>
        Computed,

        /// <summary>
        /// Переопределение для интерфейса статично и высчитывается ровно один раз
        /// </summary>
        Static
    }
}