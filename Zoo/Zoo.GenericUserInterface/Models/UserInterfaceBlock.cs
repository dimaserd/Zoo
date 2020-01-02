using System.Collections.Generic;
using Zoo.GenericUserInterface.Enumerations;

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
        /// Значения для выпадающего списка
        /// </summary>
        public List<MySelectListItem> SelectList { get; set; }

        /// <summary>
        /// Json с кастомными данными, можно использовать для построения кастомного инпута
        /// </summary>
        public string CustomDataJson { get; set; }
    }
}