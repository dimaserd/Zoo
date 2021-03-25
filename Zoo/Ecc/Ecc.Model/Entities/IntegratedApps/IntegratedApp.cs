using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecc.Common.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.IntegratedApps
{
    [Table(nameof(IntegratedApp), Schema = Schemas.EccSchema)]
    public class IntegratedApp
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IntegratedAppType AppType { get; set; }

        [MaxLength(128)]
        public string Uid { get; set; }

        public string ConfigurationJson { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegratedApp>()
                .HasIndex(p => new { p.Uid }).IsUnique();
        }
    }
}