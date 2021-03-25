using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Logic.Models.Messaging;
using Ecc.Logic.Workers.EmailRedirects;
using Ecc.Model.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Messaging
{
    public class MailDistributionQueryWorker : ApplicationInteractionWorker
    {
        EmailRedirectsQueryWorker EmailRedirectsQueryWorker { get; }

        public MailDistributionQueryWorker(ICrocoAmbientContextAccessor ambientContext, 
            ICrocoApplication application,
            EmailRedirectsQueryWorker emailRedirectsQueryWorker) : base(ambientContext, application)
        {
            EmailRedirectsQueryWorker = emailRedirectsQueryWorker;
        }

        public async Task<MailMessageDetailedModel> GetMailMessageDetailed(string id)
        {
            var result = await Query<MailMessageInteraction>().Select(MailMessageDetailedModel.SelectExpression).FirstOrDefaultAsync(x => x.Id == id);

            if(result != null)
            {
                result.Redirects = await EmailRedirectsQueryWorker.GetCatchesByEmailId(id);
            }

            return result;
        }

        public Task<GetListResult<MailMessageModel>> GetClientMailMessages(GetClientInteractions model)
        {
            var queryWithStatus = GetQueryWithStatus(Query<MailMessageInteraction>().BuildQuery(model.GetCriterias()));

            return EFCoreExtensions.GetAsync(model, queryWithStatus.OrderByDescending(x => x.Interaction.CreatedOn), MailMessageModel.SelectExpression);
        }
    }
}