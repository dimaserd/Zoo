namespace Ecc.Contract.Models
{
    /// <summary>
    /// Отправить Email
    /// </summary>
    public class SendMailMessage
    {
        /// <summary>
        /// Тело письма
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Тема письма
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Идентификатор рассылки для сообщений
        /// </summary>
        public string MessageDistributionId { get; set; }

        /// <summary>
        /// Список идентификаторов файлов с вложениями
        /// </summary>
        public int[] AttachmentFileIds { get; set; }
    }
}