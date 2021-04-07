using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Email
{
    /// <summary>
    /// Сущнось Рассылка сообщений
    /// </summary>
    [Table(nameof(MessageDistribution), Schema = Schemas.EccSchema)]
    public class MessageDistribution
    {
        /// <summary>
        /// Идентификатор рассылки
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Тип рассылки
        /// </summary>
        [MaxLength(50)]
        public string Type { get; set; }

        /// <summary>
        /// Дополнительные данные
        /// </summary>
        public string Data { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<MessageDistribution>();
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasIndex(x => x.Type).IsUnique(false);
        }
    }
}