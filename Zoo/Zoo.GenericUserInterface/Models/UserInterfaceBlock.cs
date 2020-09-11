﻿using Zoo.GenericUserInterface.Enumerations;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Блок описывающий свойство в модели
    /// </summary>
    public class UserInterfaceBlock
    {
        /// <summary>
        /// Название свойства
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Текст лейбла
        /// </summary>
        public string LabelText { get; set; }

        /// <summary>
        /// Тип пользовательского интрефейса
        /// </summary>
        public UserInterfaceType InterfaceType { get; set; }

        /// <summary>
        /// Название кастомного типа пользовательского интерфейса
        /// </summary>
        public string CustomUserInterfaceType { get; set; }

        /// <summary>
        /// Данные для построения выпадающего списка
        /// </summary>
        public DropDownListData DropDownData { get; set; }

        /// <summary>
        /// Дополнительная информация для текстового блока
        /// </summary>
        public UserInterfaceTextBoxData TextBoxData { get; set; }

        /// <summary>
        /// Json с кастомными данными, можно использовать для построения кастомного инпута
        /// </summary>
        public string CustomDataJson { get; set; }

        /// <summary>
        /// Вложенный интерфейс
        /// </summary>
        public GenericInterfaceModel InnerGenericInterface { get; set; }

        /// <summary>
        /// Данные для построения автокомплита
        /// </summary>
        public AutoCompleteData AutoCompleteData { get; set; }
    }
}