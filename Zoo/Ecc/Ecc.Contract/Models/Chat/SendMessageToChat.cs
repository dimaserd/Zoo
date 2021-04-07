namespace Ecc.Contract.Models.Chat
{
    /// <summary>
    /// Отправить сообщение в чат
    /// </summary>
    public class SendMessageToChat
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }
}