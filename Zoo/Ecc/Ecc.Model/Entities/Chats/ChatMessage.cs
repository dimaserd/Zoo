using Ecc.Model.Entities.External;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Chats
{
    /// <summary>
    /// Сообщение в чате
    /// </summary>
    [Table(nameof(EccChatMessage), Schema = Schemas.EccSchema)]
    public class EccChatMessage
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public long SentOnUtcTicks { get; set; }

        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Чат
        /// </summary>
        [ForeignKey(nameof(ChatId))]
        public virtual EccChat Chat { get; set; }

        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public string SenderUserId { get; set; }

        /// <summary>
        /// Отправитель
        /// </summary>
        [ForeignKey(nameof(SenderUserId))]
        public virtual EccUser SenderUser { get; set; }

        /// <summary>
        /// Вложения
        /// </summary>
        public virtual ICollection<EccChatMessageAttachment> Attachments { get; set; }
    }
}