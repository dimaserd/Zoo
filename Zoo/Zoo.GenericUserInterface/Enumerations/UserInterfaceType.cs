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
        /// Поле для ввода числа
        /// </summary>
        NumberBox,

        /// <summary>
        /// Форма для ввода пароля
        /// </summary>
        Password,

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

        /// <summary>
        /// Вложенный новый интерфейс
        /// </summary>
        GenericInterfaceForClass,

        /// <summary>
        /// Вложенный новый интерфейс для массива
        /// </summary>
        GenericInterfaceForArray,

        /// <summary>
        /// Автокомплит для единичного выбора (Поддерживается только для примитивных типов)
        /// </summary>
        AutoCompleteForSingle,

        /// <summary>
        /// Автокомплит для мультивыбора (Поддерживается только для масиива из примитивных типа)
        /// </summary>
        AutoCompleteForMultiple
    }
}