using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Services.Base;
using Ecc.Model.Entities.Email;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecc.Logic.Services.Emails
{
    /// <summary>
    /// Сервис для работы с запросами к группам электронных адресов
    /// </summary>
    public class EmailGroupQueryWorker : BaseEccService
    {
        static readonly Expression<Func<EmailGroup, EmailGroupModel>> SelectExpression = x => new EmailGroupModel
        {
            Id = x.Id,
            Name = x.Name,
            EmailsCount = x.Emails.Count
        };

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public EmailGroupQueryWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        /// <summary>
        /// Получить электронные адреса в группе
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<string>> GetEmailsInGroup(GetEmailsInGroup model)
        {
            return EFCoreExtensions.GetAsync(model, Query<EmailInEmailGroupRelation>().Where(x => x.EmailGroupId == model.EmailGroupId).OrderByDescending(x => x.Email), x => x.Email);
        }

        /// <summary>
        /// Получить группы адресов
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<EmailGroupModel>> GetEmailGroups(GetListSearchModel model)
        {
            return EFCoreExtensions.GetAsync(model, Query<EmailGroup>().OrderByDescending(x => x.CreatedOn), SelectExpression);
        }
    }
}