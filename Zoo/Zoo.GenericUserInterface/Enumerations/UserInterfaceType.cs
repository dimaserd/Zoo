namespace Zoo.GenericUserInterface.Enumerations
{
    /// <summary>
    /// Тип пользовательского интерейса для свойства
    /// </summary>
    public enum UserInterfaceType
    {
        /// <summary>
        /// Кастомный элемент ввода
        /// </summary>
        CustomInput,

        /// <summary>
        /// Текстовое поле
        /// </summary>
        TextBox,

        /// <summary>
        /// Большое текстовое поле
        /// </summary>
        TextArea,

        /// <summary>
        /// Выпадающий список
        /// </summary>
        DropDownList,

        /// <summary>
        /// Скрытый инпут со значением
        /// </summary>
        Hidden,

        /// <summary>
        /// Календарь для даты
        /// </summary>
        DatePicker,

        /// <summary>
        /// Выпадающий список с множественным выбором
        /// </summary>
        MultipleDropDownList,
    }
}