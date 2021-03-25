using System.ComponentModel.DataAnnotations;

namespace Ecc.Common.Enumerations
{
    public enum IntegratedAppType
    {
        [Display(Name = "iOS Application")]
        IosApplication,

        [Display(Name = "Android Application")]
        AndroidApplication
    }
}