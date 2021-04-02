using Croco.Core.Contract.Files;

namespace Ecc.Contract.Models.Emails
{
    /// <summary>
    /// Отправить эмейл с предзагруженными вложениями
    /// </summary>
    public class SendEmailModelWithLoadedAttachments
    {
        /// <summary>
        /// Тема письма
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Тело письма
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Вложения
        /// </summary>
        public IFileData[] AttachmentFiles { get; set; }
    }
}