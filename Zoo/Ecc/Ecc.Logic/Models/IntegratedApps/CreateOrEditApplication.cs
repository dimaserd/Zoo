using System.ComponentModel.DataAnnotations;
using Ecc.Common.Enumerations;

namespace Ecc.Logic.Models.IntegratedApps
{
    /// <summary>
    /// Модель для создания и редактирования интеграционного приложения
    /// </summary>
    public class CreateOrEditApplication
    {
        /// <summary>
        /// Название
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать название приложения")]
        public string Name { get; set; }
        
        /// <summary>
        /// Описание
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать описание приложения")]
        public string Description { get; set; }

        /// <summary>
        /// Тип приложения
        /// </summary>
        public IntegratedAppType AppType { get; set; }

        /// <summary>
        /// Уникальный идентификатор приложения
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать уникальный идентификатор приложения")]
        [MaxLength(128)]
        public string Uid { get; set; }

        /// <summary>
        /// Настройки конфигурации
        /// </summary>
        public string ConfigurationJson { get; set; }
    }
}