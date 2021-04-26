using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.External
{
    /// <summary>
    /// Сущность описывающая группу пользователей
    /// </summary>
    [Table(nameof(EccUserGroup), Schema = Schemas.EccSchema)]
    public class EccUserGroup
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Флаг удаленности
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Пользователи принадлежащие к данной группе
        /// </summary>
        public ICollection<EccUserInUserGroupRelation> Users { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EccUserGroup>()
                .HasIndex(p => new { p.Name }).IsUnique();
        }
    }
}