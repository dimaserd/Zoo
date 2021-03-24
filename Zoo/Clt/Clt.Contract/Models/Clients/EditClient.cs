using System;
using System.ComponentModel.DataAnnotations;
using Clt.Contract.Resources;

namespace Clt.Contract.Models.Users
{
    public class EditClient
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(AnnotationsErrorMessages.UserNameShouldBeDefined), ErrorMessageResourceType = typeof(AnnotationsErrorMessages))]
        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public bool? Sex { get; set; }

        public string ObjectJson { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(AnnotationsErrorMessages.PhoneNumberShouldBeDefined), ErrorMessageResourceType = typeof(AnnotationsErrorMessages))]
        public string PhoneNumber { get; set; }
    }
}