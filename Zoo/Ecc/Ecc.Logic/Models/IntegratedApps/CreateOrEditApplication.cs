using System.ComponentModel.DataAnnotations;
using Ecc.Common.Enumerations;

namespace Ecc.Logic.Models.IntegratedApps
{
    public class CreateOrEditApplication
    {
        [Required(ErrorMessage = "Необходимо указать название приложения")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Необходимо указать описание приложения")]
        public string Description { get; set; }

        public IntegratedAppType AppType { get; set; }

        [Required(ErrorMessage = "Необходимо указать уникальный идентификатор приложения")]
        [MaxLength(128)]
        public string Uid { get; set; }

        public string ConfigurationJson { get; set; }
    }
}