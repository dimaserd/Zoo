using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    /// <summary>
    /// Модель для изменения пароля
    /// </summary>
    public class ChangeUserPasswordModel
    {
        /// <summary>
        /// Старый пароль
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать старый пароль")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Новый пароль
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать новый пароль")]
        public string NewPassword { get; set; }
    }
}