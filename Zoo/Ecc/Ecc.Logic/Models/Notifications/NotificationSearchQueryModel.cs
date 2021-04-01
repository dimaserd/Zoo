using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Model.Entities.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ecc.Logic.Models.Notifications
{
    /// <summary>
    /// Модель для поиска уведомлений
    /// </summary>
    public class NotificationSearchQueryModel : GetListSearchModel
    {
        /// <summary>
        /// Идентификатор пользователя, к которому прикреплено уведомление
        /// </summary>
        [Description("Идентификатор пользователя, к которому прикреплено уведомление")]
        public string UserId { get; set; }

        /// <summary>
        /// Диапазон для даты создания уведомления
        /// </summary>
        [Description("Диапазон для даты создания уведомления")]
        public GenericRange<DateTime> CreatedOn { get; set; }

        /// <summary>
        /// Флаг прочитанности
        /// </summary>
        public bool? Read { get; set; }

        /// <summary>
        /// Получить критерии для поиска
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SearchQueryCriteria<UserNotificationInteraction>> GetCriterias()
        {
            yield return Read.MapNullable(value => new SearchQueryCriteria<UserNotificationInteraction>(x => x.ReadOn.HasValue == value));
            yield return CreatedOn.GetSearchCriteriaFromGenericRange<UserNotificationInteraction, DateTime>(x => x.CreatedOn);
            yield return UserId.MapString(str => new SearchQueryCriteria<UserNotificationInteraction>(x => x.UserId == str));
        }
    }
}