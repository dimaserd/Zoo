using System.ComponentModel.DataAnnotations.Schema;
using Ecc.Model.Entities.External;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.Ecc.Messaging
{
    /// <summary>
    /// Сущность описывающая привзяку рассылки к группе пользователей
    /// </summary>
    [Table(nameof(MailDistributionUserGroupRelation), Schema = Schemas.EccSchema)]
    public class MailDistributionUserGroupRelation
    {
        /// <summary>
        /// Идентификатор рассылки
        /// </summary>
        [ForeignKey(nameof(MailDistribution))]
        public string MailDistributionId { get; set; }

        /// <summary>
        /// Рассылка
        /// </summary>
        public virtual MailDistribution MailDistribution { get; set; }

        /// <summary>
        /// Идентификатор группы
        /// </summary>
        [ForeignKey(nameof(UserGroup))]
        public string GroupId { get; set; }

        /// <summary>
        /// Группа пользователей
        /// </summary>
        public virtual EccUserGroup UserGroup { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MailDistributionUserGroupRelation>()
                .HasKey(p => new { p.MailDistributionId, p.GroupId });
        }
    }
}