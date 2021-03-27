using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    /// <summary>
    /// Сбросить пароль пользователя администратором
    /// </summary>
    public class ResetPasswordByAdminModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }
    }
}