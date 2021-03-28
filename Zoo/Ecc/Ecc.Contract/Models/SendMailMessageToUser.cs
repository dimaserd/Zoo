namespace Ecc.Contract.Models
{
    /// <summary>
    /// Отправить Email пользоателю
    /// </summary>
    public class SendMailMessageToUser
    {
        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Тема сообщения
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Список идентификаторов файлов с вложениями
        /// </summary>
        public int[] AttachmentFileIds { get; set; }
    }
}