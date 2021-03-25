using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.External
{
    [Table(nameof(EccUserGroup), Schema = Schemas.EccSchema)]
    public class EccUserGroup
    {
        public string Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }

        /// <summary>
        /// Пользователи принадлежащие к данной группе
        /// </summary>
        public ICollection<EccUserInUserGroupRelation> Users { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EccUserGroup>()
                .HasIndex(p => new { p.Name }).IsUnique();
        }
    }
}