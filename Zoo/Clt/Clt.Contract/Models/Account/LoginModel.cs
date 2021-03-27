using Clt.Logic.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    /// <summary>
    /// Модель описывающая логин через Email
    /// </summary>
    public class LoginModel : LoginModelBase
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать адрес электронной почты")]
        [Display(Name = "Адрес электронной почты")]
        [EmailAddress]
        public string Email { get; set; }
    }
}