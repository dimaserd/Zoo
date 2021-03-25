using System.Collections.Generic;

namespace Doc.Logic.Word.Models
{
    /// <summary>
    /// Модеь описывающая процессинг docx документа
    /// </summary>
    public class DocXDocumentObjectModel
    {
        /// <summary>
        /// Шаблон документа для преобразования
        /// </summary>
        public string DocumentTemplateFileName { get; set; }

        /// <summary>
        /// Куда сохранить документ
        /// </summary>
        public string DocumentSaveFileName { get; set; }

        /// <summary>
        /// Текстовые замены
        /// </summary>
        public Dictionary<string, string> Replaces { get; set; }

        /// <summary>
        /// Замены для изображений, ключ - вместо чего вставить, значение - путь к локальному изображению
        /// </summary>
        public List<DocxImageReplace> ToReplaceImages { get; set; }

        /// <summary>
        /// Замены
        /// </summary>
        public List<DocumentTable> Tables { get; set; }
    }
}