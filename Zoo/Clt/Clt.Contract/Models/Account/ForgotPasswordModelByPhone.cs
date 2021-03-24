using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    public class ForgotPasswordModelByPhone
    {
        [Required(ErrorMessage = "Необходимо указать номер телефона")]
        public string PhoneNumber { get; set; }
    }
}