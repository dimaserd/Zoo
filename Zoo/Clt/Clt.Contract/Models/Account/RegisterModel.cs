using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    /// <summary>
    /// Модель для регистрации пользователя
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Display(Name = "Имя")]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать пароль")]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}