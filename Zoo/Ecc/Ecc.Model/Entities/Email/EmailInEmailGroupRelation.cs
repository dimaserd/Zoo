using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Email
{
    /// <summary>
    /// Сущность описывающая принадлежность адреса электронной почты к группе
    /// </summary>
    [Table(nameof(EmailInEmailGroupRelation), Schema = Schemas.EccSchema)]
    public class EmailInEmailGroupRelation
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public string EmailGroupId { get; set; }

        /// <summary>
        /// Ссылка на группу эмейлов
        /// </summary>
        [ForeignKey(nameof(EmailGroupId))]
        public virtual EmailGroup EmailGroup { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [MaxLength(128)]
        public string Email { get; set; }

        internal static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmailInEmailGroupRelation>()
                .HasIndex(x => new { x.EmailGroupId, x.Email })
                .IsUnique();
        }
    }
}