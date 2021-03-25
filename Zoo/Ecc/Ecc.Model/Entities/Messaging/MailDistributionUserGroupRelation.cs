using System.ComponentModel.DataAnnotations.Schema;
using Ecc.Model.Entities.External;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.Ecc.Messaging
{
    [Table(nameof(MailDistributionUserGroupRelation), Schema = Schemas.EccSchema)]
    public class MailDistributionUserGroupRelation
    {
        [ForeignKey(nameof(MailDistribution))]
        public string MailDistributionId { get; set; }

        public virtual MailDistribution MailDistribution { get; set; }

        [ForeignKey(nameof(UserGroup))]
        public string GroupId { get; set; }

        public virtual EccUserGroup UserGroup { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MailDistributionUserGroupRelation>()
                .HasKey(p => new { p.MailDistributionId, p.GroupId });
        }
    }
}