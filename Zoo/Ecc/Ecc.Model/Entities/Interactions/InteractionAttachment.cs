using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecc.Model.Entities.External;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ecc.Model.Entities.Interactions
{
    [Table(nameof(InteractionAttachment), Schema = Schemas.EccSchema)]
    public class InteractionAttachment
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey(nameof(Interaction))]
        public string InteractionId { get; set; }

        [JsonIgnore]
        public virtual Interaction Interaction { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(File))]
        public int FileId { get; set; }

        [JsonIgnore]
        public virtual EccFile File { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InteractionAttachment>()
                .HasKey(p => new { p.InteractionId, p.FileId, });
        }
    }
}