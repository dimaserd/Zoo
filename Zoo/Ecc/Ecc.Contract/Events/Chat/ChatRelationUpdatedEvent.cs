namespace Ecc.Contract.Events.Chat
{
    public class ChatRelationUpdatedEvent
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }
    }
}