namespace Doc.Logic.Word.Models
{
    /// <summary>
    /// Настройки для отрисовки таблицы
    /// </summary>
    public class DocTableSettings
    {
        /// <summary>
        /// Цвет границы
        /// </summary>
        public string BorderColor { get; set; }

        /// <summary>
        /// Размер границы
        /// </summary>
        public uint BorderSize { get; set; }

        /// <summary>
        /// Размер шрифта заголовка
        /// </summary>
        public int HeaderFontSize { get; set; }

        /// <summary>
        /// Использовать жирные заголовки
        /// </summary>
        public bool BoldHeader { get; set; }

        /// <summary>
        /// Размер шрифта в таблице
        /// </summary>
        public int TableRowFontSize { get; set; }

        /// <summary>
        /// Жирный ряд в таблице
        /// </summary>
        public bool BoldTableRow { get; set; }
    }
}