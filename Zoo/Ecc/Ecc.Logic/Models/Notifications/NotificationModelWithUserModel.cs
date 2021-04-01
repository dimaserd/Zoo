using Ecc.Contract.Models;
using Ecc.Model.Entities.Interactions;
using System;
using System.Linq.Expressions;

namespace Ecc.Logic.Models.Notifications
{
    /// <summary>
    /// Модель пользователя с уведомлением
    /// </summary>
    public class NotificationModelWithUserModel
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public EccUserModel User { get; set; }

        /// <summary>
        /// Уведомление
        /// </summary>
        public NotificationModel Notification { get; set; }

        internal static Expression<Func<UserNotificationInteraction, NotificationModelWithUserModel>> SelectExpression = x => new NotificationModelWithUserModel
        {
            Notification = new NotificationModel
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
                ReadOn = x.ReadOn,
                Text = x.MessageText,
                Title = x.TitleText,
                UserId = x.UserId,
                ObjectJson = x.ObjectJson,
                Type = x.NotificationType
            },
            
            User = new EccUserModel
            {
                Id = x.User.Id,
                Email = x.User.Email,
                PhoneNumber = x.User.PhoneNumber
            },
        };
    }
}