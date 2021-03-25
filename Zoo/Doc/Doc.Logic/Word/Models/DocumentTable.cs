using System.Collections.Generic;

namespace Doc.Logic.Word.Models
{
    /// <summary>
    /// Документ описывающий таблицу
    /// </summary>
    public class DocumentTable
    {
        /// <summary>
        /// Вместо данного текста будет установлена таблица
        /// </summary>
        public string PlacingText { get; set; }

        /// <summary>
        /// Заголовок таблицы
        /// </summary>
        public List<string> Header { get; set; }

        /// <summary>
        /// Данные в таблице
        /// </summary>
        public List<List<string>> Data { get; set; }
    }
}