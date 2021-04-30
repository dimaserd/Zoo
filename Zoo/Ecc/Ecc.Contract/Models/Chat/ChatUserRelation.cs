namespace Ecc.Contract.Models.Chat
{
    /// <summary>
    /// Модель описывающая пользователя в чате
    /// </summary>
    public class ChatUserRelation
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Дата последнего визита в тиках по UTC 
        /// </summary>
        public long LastVisitUtcTicks { get; set; }
    }
}