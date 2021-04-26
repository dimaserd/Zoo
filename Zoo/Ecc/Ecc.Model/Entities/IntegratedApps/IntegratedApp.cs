using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecc.Common.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Entities.IntegratedApps
{
    /// <summary>
    /// Сущность описывающая интеграционное приложение
    /// </summary>
    [Table(nameof(IntegratedApp), Schema = Schemas.EccSchema)]
    public class IntegratedApp
    {
        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название приложения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип приложения
        /// </summary>
        public IntegratedAppType AppType { get; set; }

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [MaxLength(128)]
        public string Uid { get; set; }

        /// <summary>
        /// Настройки конфигурации
        /// </summary>
        public string ConfigurationJson { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegratedApp>()
                .HasIndex(p => new { p.Uid }).IsUnique();
        }
    }
}