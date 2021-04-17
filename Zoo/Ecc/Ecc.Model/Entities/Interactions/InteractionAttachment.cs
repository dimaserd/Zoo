using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecc.Model.Entities.External;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.Interactions
{
    /// <summary>
    /// Сущность описывающая вложение
    /// </summary>
    [Table(nameof(InteractionAttachment), Schema = Schemas.EccSchema)]
    public class InteractionAttachment
    {
        /// <summary>
        /// Идентификтаор взаимодействия
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [ForeignKey(nameof(Interaction))]
        public string InteractionId { get; set; }

        /// <summary>
        /// Вхаимодействие
        /// </summary>
        public virtual Interaction Interaction { get; set; }

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(File))]
        public int FileId { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual EccFile File { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InteractionAttachment>()
                .HasKey(p => new { p.InteractionId, p.FileId, });
        }
    }
}