namespace Ecc.Contract.Models.EmailGroup
{
    /// <summary>
    /// Отослать емейлы для группы
    /// </summary>
    public class SendMailsForEmailGroup
    {
        /// <summary>
        /// Идентификтатор группы
        /// </summary>
        public string EmailGroupId { get; set; }

        /// <summary>
        /// Тема сообщения
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Идентификаторы файлов
        /// </summary>
        public int[] AttachmentFileIds { get; set; }
    }
}