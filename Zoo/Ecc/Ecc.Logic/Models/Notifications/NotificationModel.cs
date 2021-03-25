using System;
using System.Linq.Expressions;
using Ecc.Common.Enumerations;
using Ecc.Model.Entities.Interactions;

namespace Ecc.Logic.Models.Notifications
{
    public class NotificationModel
    {
        /// <summary>
        /// Идентификатор уведомления
        /// </summary>
        public string Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string ObjectJson { get; set; }

        public UserNotificationType Type { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ReadOn { get; set; }

        public string UserId { get; set; }

        public static Expression<Func<UserNotificationInteraction, NotificationModel>> SelectExpression = x => new NotificationModel
        {
            Title = x.TitleText,
            CreatedOn = x.CreatedOn,
            ReadOn = x.ReadOn,
            Id = x.Id,
            ObjectJson = x.ObjectJson,
            Text = x.TitleText,
            Type = x.NotificationType,
            UserId = x.UserId
        };
    }
}