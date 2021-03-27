using System.ComponentModel.DataAnnotations;

namespace Tms.Logic.Models
{
    /// <summary>
    /// Модель для отображения пользователя
    /// </summary>
    public class UserFullNameEmailAndAvatarModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }

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
        /// Адрес электронной почты
        /// </summary>
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Display(Name = "Имя")]
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор аватара
        /// </summary>
        [Display(Name = "Идентификатор аватара")]
        public int? AvatarFileId { get; set; }
    }
}