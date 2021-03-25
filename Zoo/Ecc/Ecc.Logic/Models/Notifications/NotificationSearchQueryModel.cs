using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Model.Entities.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ecc.Logic.Models.Notifications
{
    public class NotificationSearchQueryModel : GetListSearchModel
    {
        [Description("Идентификатор пользователя, к которому прикреплено уведомление")]
        public string UserId { get; set; }

        [Description("Диапазон для даты создания уведомления")]
        public GenericRange<DateTime> CreatedOn { get; set; }

        public bool? Read { get; set; }

        public IEnumerable<SearchQueryCriteria<UserNotificationInteraction>> GetCriterias()
        {
            yield return Read.MapNullable(value => new SearchQueryCriteria<UserNotificationInteraction>(x => x.ReadOn.HasValue == value));
            yield return CreatedOn.GetSearchCriteriaFromGenericRange<UserNotificationInteraction, DateTime>(x => x.CreatedOn);
            yield return UserId.MapString(str => new SearchQueryCriteria<UserNotificationInteraction>(x => x.UserId == str));
        }
    }
}