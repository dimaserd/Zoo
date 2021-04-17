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
        /// <summary>
        /// Идентификатор рассылки
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
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

        /// <summary>
        /// Отправлять каждому пользователю
        /// </summary>
        public bool SendToEveryUser { get; set; }

        /// <summary>
        /// Группы пользователей
        /// </summary>
        public ICollection<MailDistributionUserGroupRelation> UserGroups { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MailDistribution>()
                .HasIndex(p => new { p.Name }).IsUnique();
        }
    }
}