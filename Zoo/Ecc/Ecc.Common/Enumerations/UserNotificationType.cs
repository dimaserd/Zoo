using System.ComponentModel.DataAnnotations;

namespace Ecc.Common.Enumerations
{
    public enum UserNotificationType
    {
        [Display(Name = "Успешное")]
        Success,

        [Display(Name = "Информационное")]
        Info,

        [Display(Name = "Предупреждение")]
        Warning,

        [Display(Name = "Опасность")]
        Danger,

        [Display(Name = "Кастом")]
        Custom
    }
}