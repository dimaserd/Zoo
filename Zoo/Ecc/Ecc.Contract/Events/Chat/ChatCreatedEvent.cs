using System.Collections.Generic;

namespace Ecc.Contract.Events.Chat
{
    /// <summary>
    /// Событие о создании чата
    /// </summary>
    public class ChatCreatedEvent
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Идентификаторы пользователей
        /// </summary>
        public List<string> UserIds { get; set; }
    }
}