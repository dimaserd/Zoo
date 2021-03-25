using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Logic.Models.Users;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Interactions;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Logic.Workers.Emails
{
    public class UserMailMessageWorker : BaseEccWorker
    {
        public UserMailMessageWorker(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application) : base(ambientContext, application)
        {
        }

        public Task<GetListResult<UserMailMessageModel>> GetMailsAsync(GetListSearchModel model)
        {
            var initQuery = Query<MailMessageInteraction>().OrderByDescending(x => x.CreatedOn);

            return EFCoreExtensions.GetAsync(model, initQuery, UserMailMessageModel.SelectExpression);
        }

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