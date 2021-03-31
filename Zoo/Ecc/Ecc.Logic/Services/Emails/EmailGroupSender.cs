using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Services.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecc.Logic.Services.Emails
{
    /// <summary>
    /// Отправитель Email группе пользователей
    /// </summary>
    public class EmailGroupSender : BaseEccService
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public EmailGroupSender(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        /// <summary>
        /// Начать рассылку для группы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> StartEmailDistributionForGroup(SendMailsForEmailGroup model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (!await Query<EmailGroup>().AnyAsync(x => x.Id == model.EmailGroupId))
            {
                return new BaseApiResponse(false, "Группа не найдена по указанному идентификатору");
            }

            if (!await Query<EmailInEmailGroupRelation>().AnyAsync(x => x.EmailGroupId == model.EmailGroupId))
            {
                return new BaseApiResponse(false, "Эмейлы не существуют в данной группе");
            }

            await PublishMessageAsync(model);

            return new BaseApiResponse(true, "Начата рассылка сообщений по данной группе");
        }
    }
}