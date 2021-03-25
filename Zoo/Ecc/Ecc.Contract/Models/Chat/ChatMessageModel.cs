using System.ComponentModel;

namespace Ecc.Contract.Models.Chat
{
    public class ChatMessageModel
    {
        /// <summary>
        /// Текст сообщения
        /// </summary>
        [Description("Текст сообщения")]
        public string Message { get; set; }

        /// <summary>
        /// Дата отправки в тиках
        /// </summary>
        [Description("Дата отправки в тиках")]
        public long SentOnUtcTicks { get; set; }

        /// <summary>
        /// Идентификатор отправителя сообщения
        /// </summary>
        [Description("Идентификатор отправителя сообщения")]
        public string SenderUserId { get; set; }
    }
}