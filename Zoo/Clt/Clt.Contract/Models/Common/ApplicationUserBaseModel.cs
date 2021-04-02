using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Common
{
    /// <summary>
    /// Базовая модель пользователя
    /// </summary>
    public class ApplicationUserBaseModel : UserWithNameAndEmailAvatarModel
    {
        /// <summary>
        /// Email подтвержден
        /// </summary>
        [Description("Email подтвержден")]
        [Display(Name = "Email подтвержден")]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [Description("Номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Номер телефона подтвержден
        /// </summary>
        [Description("Номер телефона подтвержден")]
        [Display(Name = "Номер телефона подтвержден")]
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Штамп безопасности
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Деактивирован
        /// </summary>
        [Display(Name = "Деактивирован")]
        public bool DeActivated { get; set; }
        
        /// <summary>
        /// Роли пользователя
        /// </summary>
        [Display(Name = "Права пользователя")]
        public List<string> RoleNames { get; set; }

        /// <summary>
        /// Хеш пароля
        /// </summary>
        public string PasswordHash { get; set; }
    }
}