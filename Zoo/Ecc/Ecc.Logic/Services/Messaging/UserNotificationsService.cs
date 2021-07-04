using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ecc.Logic.Models.Notifications;
using Ecc.Model.Entities.Interactions;
using Ecc.Model.Entities.External;
using Ecc.Logic.Services.Base;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;

namespace Ecc.Logic.Services.Messaging
{
    /// <summary>
    /// Сервис для работы с системными уведомлениями
    /// </summary>
    public class UserNotificationsService : BaseEccService
    {
        /// <summary>
        /// Удалить уведомление
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> RemoveNotificationAsync(string id)
        {
            if (!IsUserAdmin())
            {
                return new BaseApiResponse(false, "Вы не обладаете правами администратора");
            }

            var repo = GetRepository<UserNotificationInteraction>();

            var notification = await repo.Query().FirstOrDefaultAsync(x => x.Id == id);

            if (notification == null)
            {
                return new BaseApiResponse(false, "Уведомление не найдено по указанному идентификатору");
            }

            if (notification.ReadOn.HasValue)
            {
                return new BaseApiResponse(false, "Вы не можете удалить прочитанное уведомление");
            }

            repo.DeleteHandled(notification);

            return await TrySaveChangesAndReturnResultAsync("Уведомление удалено");
        }

        /// <summary>
        /// Создать уведомление
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> CreateNotificationAsync(CreateNotification model)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (!await Query<EccUser>().AnyAsync(x => x.Id == model.UserId))
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            CreateHandled(new UserNotificationInteraction
            {
                UserId = model.UserId,
                ReadOn = null,
                TitleText = model.Title,
                MessageText = model.Text,
                NotificationType = model.Type,
                ObjectJson = model.ObjectJSON,
            });

            return await TrySaveChangesAndReturnResultAsync("Создано уведомление для пользователя");
        }

        /// <summary>
        /// Пометить уведомление как прочитанное
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> MarkNotificationAsReadAsync(string id)
        {
            var repo = GetRepository<UserNotificationInteraction>();

            var notification = await repo.Query().FirstOrDefaultAsync(x => x.UserId == UserId && x.Id == id);

            if (notification == null)
            {
                return new BaseApiResponse(false, "Уведомление не найдено по указанному идентификатору");
            }

            if (notification.ReadOn.HasValue)
            {
                return new BaseApiResponse(false, "Уведомление уже является прочитанным");
            }

            notification.ReadOn = Application.DateTimeProvider.Now;

            repo.UpdateHandled(notification);

            return await TrySaveChangesAndReturnResultAsync("Уведомление помечено как прочитанное");
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        public UserNotificationsService(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application) : base(ambientContext, application)
        {
        }
    }
}