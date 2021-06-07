using Zoo.GenericUserInterface.Utils;

namespace Zoo.GenericUserInterface.Models.Definition
{
    /// <summary>
    /// Обобщенная модель данных для автокомлита
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class AutoCompleteSuggestionData<TItem>
    {
        /// <summary>
        /// Текст который будет показан
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Дополнительные данные
        /// </summary>
        public string DataJson { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public TItem Value { get; set; }

        internal AutoCompleteSuggestion ToAutoCompleteSuggestion()
        {
            return new AutoCompleteSuggestion
            {
                DataJson = DataJson,
                Text = Text,
                Value = typeof(TItem) == typeof(string) ? Value?.ToString() : Tool.JsonConverter.Serialize(Value)
            };
        }
    }
}