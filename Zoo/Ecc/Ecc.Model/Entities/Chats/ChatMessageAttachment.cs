using Ecc.Model.Entities.External;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Chats
{
    [Table(nameof(EccChatMessageAttachment), Schema = Schemas.EccSchema)]
    public class EccChatMessageAttachment
    {
        public string Id { get; set; }

        public string ChatMessageId { get; set; }

        [ForeignKey(nameof(ChatMessageId))]
        public virtual EccChatMessage ChatMessage { get; set; }

        public int FileId { get; set; }

        [ForeignKey(nameof(FileId))]
        public virtual EccFile File { get; set; }
    }
}
