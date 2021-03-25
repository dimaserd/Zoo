using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.External
{
    [Table(nameof(EccUserInUserGroupRelation), Schema = Schemas.EccSchema)]
    public class EccUserInUserGroupRelation
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual EccUser User { get; set; }

        [ForeignKey(nameof(UserGroup))]
        public string UserGroupId { get; set; }

        public virtual EccUserGroup UserGroup { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EccUserInUserGroupRelation>()
                .HasKey(p => new { p.UserId, p.UserGroupId });
        }
    }
}