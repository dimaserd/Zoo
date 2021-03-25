using System.ComponentModel;

namespace Ecc.Contract.Models.Chat
{
    public class ChatIdWithMessage
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        [Description("Идентификатор чата")]
        public int ChatId { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        [Description("Сообщение")]
        public ChatMessageModel Message { get; set; }
    }
}