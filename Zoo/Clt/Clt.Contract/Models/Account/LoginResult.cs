using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    public enum LoginResult
    {
        [Display(Name = "Ошибка")]
        Error,

        [Display(Name = "Уже авторизован")]
        AlreadyAuthenticated,

        [Display(Name = "Неудачная попытка входа")]
        UnSuccessfulAttempt,

        [Display(Name = "Электронная почта не подтверждена")]
        EmailNotConfirmed,

        [Display(Name = "Удачное логинирование")]
        SuccessfulLogin,

        [Display(Name = "Нужно подтверждение входа")]
        NeedConfirmation,

        [Display(Name = "Пользователь деактивирован")]
        UserDeactivated,
    }
}