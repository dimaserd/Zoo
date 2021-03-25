using Croco.Core.Model.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Email
{
    [Table(nameof(EmailGroup), Schema = Schemas.EccSchema)]
    public class EmailGroup : AuditableEntityBase
    {
        public string Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public virtual ICollection<EmailInEmailGroupRelation> Emails { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
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