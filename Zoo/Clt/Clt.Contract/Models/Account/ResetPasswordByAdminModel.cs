using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    public class ResetPasswordByAdminModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }
    }
}