namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Дополнительная информация о текстбоксе
    /// </summary>
    public class UserInterfaceNumberBoxData
    {
        /// <summary>
        /// Тип данных является целым числом
        /// </summary>
        public bool IsInteger { get; set; }

        /// <summary>
        /// Минимальное значение
        /// </summary>
        public string MinValue { get; set; }

        /// <summary>
        /// Максимальное значение
        /// </summary>
        public string MaxValue { get; set; }
    }
}