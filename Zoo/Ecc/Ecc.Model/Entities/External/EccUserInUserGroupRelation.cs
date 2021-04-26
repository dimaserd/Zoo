using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.External
{
    /// <summary>
    /// Сущность описывающая принадлежность пользователя к группе
    /// </summary>
    [Table(nameof(EccUserInUserGroupRelation), Schema = Schemas.EccSchema)]
    public class EccUserInUserGroupRelation
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual EccUser User { get; set; }

        /// <summary>
        /// Идентификатор группы
        /// </summary>
        [ForeignKey(nameof(UserGroup))]
        public string UserGroupId { get; set; }

        /// <summary>
        /// Группа пользователей
        /// </summary>
        public virtual EccUserGroup UserGroup { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EccUserInUserGroupRelation>()
                .HasKey(p => new { p.UserId, p.UserGroupId });
        }
    }
}