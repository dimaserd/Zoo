using Ecc.Model.Entities.External;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Chats
{
    /// <summary>
    /// Сущность описывающая вложение в сообщении
    /// </summary>
    [Table(nameof(EccChatMessageAttachment), Schema = Schemas.EccSchema)]
    public class EccChatMessageAttachment
    {
        /// <summary>
        /// Идентификатор вложения
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public string ChatMessageId { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        [ForeignKey(nameof(ChatMessageId))]
        public virtual EccChatMessage ChatMessage { get; set; }

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        [ForeignKey(nameof(FileId))]
        public virtual EccFile File { get; set; }
    }
}
