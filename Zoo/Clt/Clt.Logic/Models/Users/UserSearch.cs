using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clt.Model.Entities;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;

namespace Clt.Logic.Models.Users
{
    /// <summary>
    /// Модель для поиск пользователей
    /// </summary>
    public class UserSearch : GetListSearchModel
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
        public static UserSearch GetAllUsers => new UserSearch
        {
            Count = null,
            OffSet = 0
        };
        
        internal IEnumerable<SearchQueryCriteria<Client>> GetCriterias()
        {
            yield return Q.MapString(str => new SearchQueryCriteria<Client>(x => x.Email.Contains(str) || x.PhoneNumber.Contains(str) || x.Name.Contains(str)));

            yield return Deactivated.MapNullable(b => new SearchQueryCriteria<Client>(x => x.DeActivated == b));

            yield return RegistrationDate.GetSearchCriteriaFromGenericRange<Client, DateTime>(x => x.CreatedOn);

            if (SearchSex)
            {
                yield return new SearchQueryCriteria<Client>(x => x.Sex == Sex);
            }
        }
    }
}