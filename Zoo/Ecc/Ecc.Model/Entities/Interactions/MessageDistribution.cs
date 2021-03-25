using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Email
{
    [Table(nameof(MessageDistribution), Schema = Schemas.EccSchema)]
    public class MessageDistribution
    {
        public string Id { get; set; }
        
        [MaxLength(50)]
        public string Type { get; set; }

        public string Data { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<MessageDistribution>();
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasIndex(x => x.Type).IsUnique(false);
        }
    }
}