using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Logic.Models.Users;
using Ecc.Logic.Services.Base;
using Ecc.Model.Entities.Interactions;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Logic.Services.Emails
{
    /// <summary>
    /// Сервис для работы с отправленными Email
    /// </summary>
    public class UserMailMessageWorker : BaseEccService
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        public UserMailMessageWorker(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application) : base(ambientContext, application)
        {
        }

        /// <summary>
        /// Получить список отправленных адресов
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<UserMailMessageModel>> GetMailsAsync(GetListSearchModel model)
        {
            var initQuery = Query<MailMessageInteraction>().OrderByDescending(x => x.CreatedOn);

            return EFCoreExtensions.GetAsync(model, initQuery, UserMailMessageModel.SelectExpression);
        }

        /// <summary>
        /// Установить дату открытия письма
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> DeterminingDateOfOpening(string id)
        {
            var repo = GetRepository<MailMessageInteraction>();

            var userMailMessage = await repo.Query().FirstOrDefaultAsync(p => p.Id == id);

            if (userMailMessage == null)
            {
                return new BaseApiResponse(false, "Сообщение не найдено по указанному идентификатору");
            }

            if (userMailMessage.ReadOn.HasValue)
            {
                return new BaseApiResponse(false, "Собщение было открыто прежде");
            }

            userMailMessage.ReadOn = Application.DateTimeProvider.Now;

            repo.UpdateHandled(userMailMessage);

            return await TrySaveChangesAndReturnResultAsync("Пользователь открыл сообщение");
        }
    }
}