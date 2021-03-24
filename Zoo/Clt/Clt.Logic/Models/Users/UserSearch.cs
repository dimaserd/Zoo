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
        [Display(Name = "Поисковая строка")]
        public string Q { get; set; }

        [Display(Name = "Является ли пользователь деактивированным")]
        public bool? Deactivated { get; set; }

        [Display(Name = "Дата регистрации")]
        public GenericRange<DateTime> RegistrationDate { get; set; }

        [Display(Name = "Фильтровать по полу")]
        public bool SearchSex { get; set; }

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
        
        public IEnumerable<SearchQueryCriteria<Client>> GetCriterias()
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