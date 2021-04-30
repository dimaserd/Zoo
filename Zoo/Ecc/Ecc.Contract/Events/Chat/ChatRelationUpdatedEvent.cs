namespace Ecc.Contract.Events.Chat
{
    /// <summary>
    /// Событие о том, что пользователь изменил свои данные для чата (например посетил его)
    /// </summary>
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