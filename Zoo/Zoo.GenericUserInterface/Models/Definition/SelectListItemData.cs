namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Данные для построения выпадающего списка
    /// </summary>
    public class SelectListItemData<T>
    {
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

        /// <summary>
        /// Значение
        /// </summary>
        public T Value { get; set; }
    }
}