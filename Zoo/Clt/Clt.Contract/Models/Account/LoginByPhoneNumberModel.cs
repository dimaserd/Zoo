using Clt.Contract.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace Clt.Logic.Models.Account
{
    public class LoginByPhoneNumberModel : LoginModelBase
    {
        [Required(ErrorMessage = "Необходимо указать номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}