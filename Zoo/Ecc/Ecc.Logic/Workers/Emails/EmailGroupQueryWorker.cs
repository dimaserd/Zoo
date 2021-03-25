using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Emails
{
    public class EmailGroupQueryWorker : BaseEccWorker
    {
        static readonly Expression<Func<EmailGroup, EmailGroupModel>> SelectExpression = x => new EmailGroupModel
        {
            Id = x.Id,
            Name = x.Name,
            EmailsCount = x.Emails.Count
        };

        public EmailGroupQueryWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        public Task<GetListResult<string>> GetEmailsInGroup(GetEmailsInGroup model)
        {
            return EFCoreExtensions.GetAsync(model, Query<EmailInEmailGroupRelation>().Where(x => x.EmailGroupId == model.EmailGroupId).OrderByDescending(x => x.Email), x => x.Email);
        }

        public Task<GetListResult<EmailGroupModel>> GetEmailGroups(GetListSearchModel model)
        {
            return EFCoreExtensions.GetAsync(model, Query<EmailGroup>().OrderByDescending(x => x.CreatedOn), SelectExpression);
        }
    }
}