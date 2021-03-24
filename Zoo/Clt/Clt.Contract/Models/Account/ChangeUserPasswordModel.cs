using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    public class ChangeUserPasswordModel
    {
        [Required(ErrorMessage = "Необходимо указать старый пароль")]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "Необходимо указать новый пароль")]
        public string NewPassword { get; set; }
    }
}