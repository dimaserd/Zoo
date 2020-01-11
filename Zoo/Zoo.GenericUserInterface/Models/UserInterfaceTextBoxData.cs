namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Дополнительная информация о текстбоксе
    /// </summary>
    public class UserInterfaceTextBoxData
    {
        /// <summary>
        /// Тип данных является целым числом
        /// </summary>
        public bool IsInteger { get; set; }

        /// <summary>
        /// Целочисленный шаг
        /// </summary>
        public int IntStep { get; set; }
    }
}