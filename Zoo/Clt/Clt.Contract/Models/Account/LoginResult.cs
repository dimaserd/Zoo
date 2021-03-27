using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    /// <summary>
    /// Перечисление описывающее результат авторизации
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        [Display(Name = "Ошибка")]
        Error,

        /// <summary>
        /// Уже авторизован
        /// </summary>
        [Display(Name = "Уже авторизован")]
        AlreadyAuthenticated,

        /// <summary>
        /// Неудачная попытка входа
        /// </summary>
        [Display(Name = "Неудачная попытка входа")]
        UnSuccessfulAttempt,

        /// <summary>
        /// Электронная почта не подтверждена
        /// </summary>
        [Display(Name = "Электронная почта не подтверждена")]
        EmailNotConfirmed,

        /// <summary>
        /// Удачное логинирование
        /// </summary>
        [Display(Name = "Удачное логинирование")]
        SuccessfulLogin,

        /// <summary>
        /// Нужно подтверждение входа
        /// </summary>
        [Display(Name = "Нужно подтверждение входа")]
        NeedConfirmation,

        /// <summary>
        /// Пользователь деактивирован
        /// </summary>
        [Display(Name = "Пользователь деактивирован")]
        UserDeactivated,
    }
}