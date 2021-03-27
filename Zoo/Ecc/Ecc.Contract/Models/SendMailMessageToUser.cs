﻿namespace Ecc.Contract.Models
{
    public class SendMailMessageToUser
    {
        public string Body { get; set; }

        public string Subject { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// Список идентификаторов файлов с вложениями
        /// </summary>
        public int[] AttachmentFileIds { get; set; }
    }
}