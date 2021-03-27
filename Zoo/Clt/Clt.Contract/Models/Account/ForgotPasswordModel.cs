using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    /// <summary>
    /// Запрос на забытие пароль
    /// </summary>
    public class ForgotPasswordModel
    {
        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Почта")]
        public string Email { get; set; }
    }
}