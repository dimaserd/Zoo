using System;

namespace Doc.Logic.Excel.Models
{
    /// <summary>
    /// Описание свойства объекта
    /// </summary>
    public class MyPropertyDescription
    {
        /// <summary>
        /// Название свойства
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Свойство для показа
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Тип данных
        /// </summary>
        public Type Type { get; set; }
    }
}