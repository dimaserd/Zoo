using System.ComponentModel.DataAnnotations;

namespace Ecc.Common.Enumerations
{
    /// <summary>
    /// Тип приложения для интеграции
    /// </summary>
    public enum IntegratedAppType
    {
        /// <summary>
        /// iOS приложение
        /// </summary>
        [Display(Name = "iOS Application")]
        IosApplication,

        /// <summary>
        /// Android приложение
        /// </summary>
        [Display(Name = "Android Application")]
        AndroidApplication
    }
}