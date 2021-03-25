using Ecc.Model.Entities.External;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Chats
{
    [Table(nameof(EccChatMessage), Schema = Schemas.EccSchema)]
    public class EccChatMessage
    {
        public string Id { get; set; }

        public string Message { get; set; }

        public long SentOnUtcTicks { get; set; }

        public int ChatId { get; set; }

        [ForeignKey(nameof(ChatId))]
        public virtual EccChat Chat { get; set; }

        public string SenderUserId { get; set; }

        [ForeignKey(nameof(SenderUserId))]
        public virtual EccUser SenderUser { get; set; }

        /// <summary>
        /// Вложения
        /// </summary>
        public virtual ICollection<EccChatMessageAttachment> Attachments { get; set; }
    }
}