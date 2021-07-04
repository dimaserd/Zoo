using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ecc.Logic.Models.Notifications;
using Croco.Core.Search.Extensions;
using Ecc.Model.Entities.Interactions;
using Ecc.Logic.Services.Base;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using System.Linq.Expressions;
using System;

namespace Ecc.Logic.Services.Messaging
{
    /// <summary>
    /// Сервис для поиска уведомлений внутри приложения
    /// </summary>
    public class UserNotificationsQueryService : BaseEccService
    {
        IOrderedQueryable<UserNotificationInteraction> GetFilteredQuery(NotificationSearchQueryModel model)
        {
            return Query<UserNotificationInteraction>()
                .BuildQuery(model.GetCriterias())
                .OrderByDescending(x => x.CreatedOn);
        }

        /// <summary>
        /// Получить список уведомлений с пользователями
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<NotificationModelWithUserModel>> GetUserNotificationsIncludingUsersAsync(NotificationSearchQueryModel model)
        {
            return EFCoreExtensions.GetAsync(model, GetFilteredQuery(model), NotificationModelWithUserModel.SelectExpression);
        }

        /// <summary>
        /// Получить список уведомлений без пользователей
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<NotificationModel>> GetUserNotificationsAsync(NotificationSearchQueryModel model)
        {
            return EFCoreExtensions.GetAsync(model, GetFilteredQuery(model), NotificationModelSelectExpression);
        }

        /// <summary>
        /// Получить последнее непрочитанное уведомление
        /// </summary>
        /// <returns></returns>
        public Task<NotificationModel> GetLastClientUnReadNotificationAsync()
        {
            return Query<UserNotificationInteraction>().Where(x => !x.ReadOn.HasValue)
                .Select(NotificationModelSelectExpression)
                .FirstOrDefaultAsync(x => x.UserId == UserId);
        }

        internal static Expression<Func<UserNotificationInteraction, NotificationModel>> NotificationModelSelectExpression = x => new NotificationModel
        {
            Title = x.TitleText,
            CreatedOn = x.CreatedOn,
            ReadOn = x.ReadOn,
            Id = x.Id,
            ObjectJson = x.ObjectJson,
            Text = x.MessageText,
            Type = x.NotificationType,
            UserId = x.UserId
        };

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public UserNotificationsQueryService(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }
    }
}