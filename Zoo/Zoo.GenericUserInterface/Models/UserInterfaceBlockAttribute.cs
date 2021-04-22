namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Атрибут примененный к определенному блоку
    /// </summary>
    public class UserInterfaceBlockAttribute
    {
        /// <summary>
        /// Название атрибута
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сериализованные данные для атрибута
        /// </summary>
        public string DataJson { get; set; }
    }
}