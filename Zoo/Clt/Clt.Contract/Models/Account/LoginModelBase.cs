using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    public class LoginModelBase
    {
        /// <summary>
        /// Пароль
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать пароль")]
        [DataType(DataType.Password)]
        [Description("Пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Запомнить пользователя (Если свойство указано как true, то пользователь будет залогинен навечно)
        /// </summary>
        [Description("Запомнить меня")]
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}