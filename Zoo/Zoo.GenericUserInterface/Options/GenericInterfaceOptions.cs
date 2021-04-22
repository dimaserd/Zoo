namespace Zoo.GenericUserInterface.Options
{
    /// <summary>
    /// Опции для создания обобщенного интерфейса
    /// </summary>
    public class GenericInterfaceOptions
    {
        /// <summary>
        /// Текст при не выбрано
        /// </summary>
        public string NotSelectedText { get; set; }

        /// <summary>
        /// Текст при true
        /// </summary>
        public string TextOnTrue { get; set; }

        /// <summary>
        /// Текст при false
        /// </summary>
        public string TextOnFalse { get; set; }

        /// <summary>
        /// Дефолтные опции
        /// </summary>
        /// <returns></returns>
        public static GenericInterfaceOptions Default()
        {
            return new GenericInterfaceOptions
            {
                NotSelectedText = "Не выбрано",
                TextOnFalse = "Нет",
                TextOnTrue = "Да"
            };
        }
    }
}