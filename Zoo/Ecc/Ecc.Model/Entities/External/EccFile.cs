using Croco.Core.Contract.Data.Entities.HaveId;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.External
{
    [Table(nameof(EccFile), Schema = Schemas.EccSchema)]
    public class EccFile : IHaveIntId
    {
        public int Id { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EccFile>().Property(x => x.Id).ValueGeneratedNever();
        }
    }
}