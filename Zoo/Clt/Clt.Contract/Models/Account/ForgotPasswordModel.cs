using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Почта")]
        public string Email { get; set; }
    }
}