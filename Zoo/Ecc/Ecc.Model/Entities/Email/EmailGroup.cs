using Croco.Core.Model.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Email
{
    /// <summary>
    /// Сущность описывающая группу эмейлов
    /// </summary>
    [Table(nameof(EmailGroup), Schema = Schemas.EccSchema)]
    public class EmailGroup : AuditableEntityBase
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Адреса электронной почты
        /// </summary>
        public virtual ICollection<EmailInEmailGroupRelation> Emails { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailGroup>().HasIndex(x => new { x.Name }).IsUnique();

            modelBuilder.Entity<EmailInEmailGroupRelation>()
                .HasOne(i => i.EmailGroup)
                .WithMany(c => c.Emails)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}