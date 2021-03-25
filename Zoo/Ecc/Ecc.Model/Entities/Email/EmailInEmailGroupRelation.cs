using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Email
{
    [Table(nameof(EmailInEmailGroupRelation), Schema = Schemas.EccSchema)]
    public class EmailInEmailGroupRelation
    {
        public string Id { get; set; }

        public string EmailGroupId { get; set; }

        [ForeignKey(nameof(EmailGroupId))]
        public virtual EmailGroup EmailGroup { get; set; }

        [MaxLength(128)]
        public string Email { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmailInEmailGroupRelation>()
                .HasIndex(x => new { x.EmailGroupId, x.Email })
                .IsUnique();
        }
    }
}