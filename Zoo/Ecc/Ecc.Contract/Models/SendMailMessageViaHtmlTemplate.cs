using System.Collections.Generic;

namespace Ecc.Contract.Models
{
    /// <summary>
    /// Отправка сообщения через Html шаблон
    /// </summary>
    public class SendMailMessageViaHtmlTemplate
    {
        /// <summary>
        /// Тема письма
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Адрес, на который нужно отправить
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Список идентификаторов файлов с вложениями
        /// </summary>
        public int[] AttachmentFileIds { get; set; }

        /// <summary>
        /// Путь к файлу шаблона
        /// </summary>
        public string TemplateFilePath { get; set; }

        /// <summary>
        /// Текстовые замены
        /// </summary>
        public List<Replacing> Replacings { get; set; } 
    }
}