using Clt.Logic.Core.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Clt.Logic.Models.Users
{
    /// <summary>
    /// Модель для редактирования пользователя
    /// </summary>
    public class EditApplicationUser
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Display(Name = "Имя")]
        public string Name { get; set; }

        /// <summary>
        /// Адрес электоронной почты
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.EmailIsRequired))]
        [EmailAddress(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.EmailIsNotValid))]
        public string Email { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; } 

        /// <summary>
        /// Пол
        /// </summary>
        [Display(Name = "Пол")]
        public bool? Sex { get; set; } 

        /// <summary>
        /// Доп данные
        /// </summary>
        public string ObjectJson { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [Display(Name = "Имя телефона")]
        public string PhoneNumber { get; set; }       
    }
}