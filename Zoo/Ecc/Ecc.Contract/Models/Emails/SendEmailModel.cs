namespace Ecc.Contract.Models.Emails
{
    /// <summary>
    /// Модель для отправки Email
    /// </summary>
    public class SendEmailModel
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
        /// АДрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Файлы вложений
        /// </summary>
        public int[] AttachmentFileIds { get; set; }
    }
}