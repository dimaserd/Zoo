using Ecc.Model.Entities.External;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Chats
{
    [Table(nameof(EccChatUserRelation), Schema = Schemas.EccSchema)]
    public class EccChatUserRelation
    {
        public int ChatId { get; set; }

        [ForeignKey(nameof(ChatId))]
        public virtual EccChat Chat { get; set; }

        public bool IsChatCreator { get; set; }

        public long LastVisitUtcTicks { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual EccUser User { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EccChatUserRelation>()
                .HasKey(x => new { x.ChatId, x.UserId });
        }
    }
}
