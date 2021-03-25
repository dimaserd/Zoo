using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.Ecc.Messaging
{
    /// <summary>
    /// Рассылка пользователям по почте
    /// </summary>
    [Table(nameof(MailDistribution), Schema = Schemas.EccSchema)]
    public class MailDistribution
    {
        public string Id { get; set; }


        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Заголовок сообщения
        /// </summary>
        [MaxLength(128)]
        public string Subject { get; set; }

        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string Body { get; set; }

        public bool SendToEveryUser { get; set; }

        public ICollection<MailDistributionUserGroupRelation> UserGroups { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MailDistribution>()
                .HasIndex(p => new { p.Name }).IsUnique();
        }
    }
}