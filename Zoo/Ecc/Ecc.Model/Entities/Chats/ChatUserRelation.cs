using Ecc.Model.Entities.External;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Chats
{
    /// <summary>
    /// Сущность описывающая принадлежность пользователя к чату
    /// </summary>
    [Table(nameof(EccChatUserRelation), Schema = Schemas.EccSchema)]
    public class EccChatUserRelation
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Ссылка на чат
        /// </summary>
        [ForeignKey(nameof(ChatId))]
        public virtual EccChat Chat { get; set; }
        
        /// <summary>
        /// Является ли пользователь создателем чата
        /// </summary>
        public bool IsChatCreator { get; set; }

        /// <summary>
        /// Дата последнего посещения
        /// </summary>
        public long LastVisitUtcTicks { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Ссылка на пользователя
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual EccUser User { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EccChatUserRelation>()
                .HasKey(x => new { x.ChatId, x.UserId });
        }
    }
}