using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Settings
{
    /// <summary>
    /// Настройки для учетных записей
    /// </summary>
    public class AccountSettingsModel
    {
        /// <summary>
        /// Разрешить авторизацию тем, кто не подтвердил почту
        /// </summary>
        [Display(Name = "Разрешить авторизацию тем, кто не подтвердил почту")]
        public bool IsLoginEnabledForUsersWhoDidNotConfirmEmail { get; set; }

        /// <summary>
        /// Должны ли пользователи подтвержать почту
        /// </summary>
        [Display(Name = "Должны ли пользователи подтвержать почту")]
        public bool ShouldUsersConfirmEmail { get; set; }

        /// <summary>
        /// Разрешена ли регистрация
        /// </summary>
        [Display(Name = "Разрешена ли регистрация")]
        public bool RegistrationEnabled { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public AccountSettingsModel()
        {
            IsLoginEnabledForUsersWhoDidNotConfirmEmail = true;
            ShouldUsersConfirmEmail = false;
            RegistrationEnabled = true;
        }
    }
}