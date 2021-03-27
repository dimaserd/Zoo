using System.ComponentModel.DataAnnotations;

namespace Ecc.Common.Enumerations
{
    /// <summary>
    /// Тип пользовательского уведомления
    /// </summary>
    public enum UserNotificationType
    {
        /// <summary>
        /// Успешное
        /// </summary>
        [Display(Name = "Успешное")]
        Success,

        /// <summary>
        /// Информационное
        /// </summary>
        [Display(Name = "Информационное")]
        Info,

        /// <summary>
        /// Предупреждение
        /// </summary>
        [Display(Name = "Предупреждение")]
        Warning,

        /// <summary>
        /// Опасность
        /// </summary>
        [Display(Name = "Опасность")]
        Danger,

        /// <summary>
        /// Кастом
        /// </summary>
        [Display(Name = "Кастом")]
        Custom
    }
}