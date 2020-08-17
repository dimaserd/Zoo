namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Элемент выпадающего списка
    /// </summary>
    public class SelectListItem
    {
        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Текст для показа
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Флаг выбранности
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Кастомные данные
        /// </summary>
        public string DataJson { get; set; }
    }
}