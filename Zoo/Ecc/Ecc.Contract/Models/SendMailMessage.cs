using System.Collections.Generic;

namespace Ecc.Contract.Models
{
    public class SendMailMessage
    {
        public string Body { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Идентификатор рассылки для сообщений
        /// </summary>
        public string MessageDistributionId { get; set; }

        /// <summary>
        /// Список идентификаторов файлов с вложениями
        /// </summary>
        public List<int> AttachmentFileIds { get; set; }
    }
}