using System;
using System.ComponentModel.DataAnnotations;
using Croco.Core.Contract.Models.Search;

namespace Clt.Contract.Models.Clients
{
    /// <summary>
    /// Модель для поиск пользователей
    /// </summary>
    public class ClientSearch : GetListSearchModel
    {
        /// <summary>
        /// Поисковая строка
        /// </summary>
        [Display(Name = "Поисковая строка")]
        public string Q { get; set; }

        /// <summary>
        /// Фильтровать по активированности
        /// </summary>
        [Display(Name = "Является ли пользователь деактивированным")]
        public bool? Deactivated { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        [Display(Name = "Дата регистрации")]
        public GenericRange<DateTime> RegistrationDate { get; set; }

        /// <summary>
        /// ФИльтровать по полу
        /// </summary>
        [Display(Name = "Фильтровать по полу")]
        public bool SearchSex { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        [Display(Name = "Пол")]
        public bool? Sex { get; set; }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        public static ClientSearch GetAllUsers => new()
        {
            Count = null,
            OffSet = 0
        };
    }
}