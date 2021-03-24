using Clt.Logic.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    public class LoginModel : LoginModelBase
    {
        public static LoginModel GetModel(LoginByPhoneNumberModel model, string email)
        {
            return new LoginModel
            {
                Email = email,
                Password = model.Password,
                RememberMe = model.RememberMe
            };
        }

        [Required(ErrorMessage = "Необходимо указать адрес электронной почты")]
        [Display(Name = "Адрес электронной почты")]
        [EmailAddress]
        public string Email { get; set; }
    }
}