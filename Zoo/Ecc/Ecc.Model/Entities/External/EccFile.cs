using Croco.Core.Contract.Data.Entities.HaveId;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.External
{
    /// <summary>
    /// Сущность описывающая файл в контексте рассылок
    /// </summary>
    [Table(nameof(EccFile), Schema = Schemas.EccSchema)]
    public class EccFile : IHaveIntId
    {
        public int Id { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EccFile>().Property(x => x.Id).ValueGeneratedNever();
        }
    }
}