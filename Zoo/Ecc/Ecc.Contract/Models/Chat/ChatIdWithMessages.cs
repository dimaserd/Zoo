using System.Collections.Generic;
using System.ComponentModel;

namespace Ecc.Contract.Models.Chat
{
    public class ChatIdWithMessages
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        [Description("Идентификатор чата")]
        public int ChatId { get; set; }

        /// <summary>
        /// Сообщения
        /// </summary>
        [Description("Сообщения")]
        public List<ChatMessageModel> Messages { get; set; }
    }
}