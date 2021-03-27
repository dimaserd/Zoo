using System.Collections.Generic;

namespace Ecc.Contract.Models
{
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

        public string TemplateFilePath { get; set; }

        public List<Replacing> Replacings { get; set; } 
    }
}