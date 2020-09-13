namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Модель данных для автокомплита
    /// </summary>
    public class AutoCompleteSuggestion
    {
        /// <summary>
        /// Текст, который будет показан
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дополнительные данные
        /// </summary>
        public string DataJson { get; set; }

        /// <summary>
        /// Сериализованное значение
        /// </summary>
        public string Value { get; set; }
    }
}