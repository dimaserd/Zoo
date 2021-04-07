using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Logic.Models;
using Ecc.Logic.Models.Messaging;
using Ecc.Logic.Services.EmailRedirects;
using Ecc.Model.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecc.Logic.Services.Messaging
{
    /// <summary>
    /// Сервис для работы с поисковыми запросами к электронным письмам
    /// </summary>
    public class MailDistributionQueryWorker : ApplicationInteractionWorker
    {
        EmailRedirectsQueryWorker EmailRedirectsQueryWorker { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        /// <param name="emailRedirectsQueryWorker"></param>
        public MailDistributionQueryWorker(ICrocoAmbientContextAccessor ambientContext, 
            ICrocoApplication application,
            EmailRedirectsQueryWorker emailRedirectsQueryWorker) : base(ambientContext, application)
        {
            EmailRedirectsQueryWorker = emailRedirectsQueryWorker;
        }

        /// <summary>
        /// Получить подробную информацию о сообщении
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MailMessageDetailedModel> GetMailMessageDetailed(string id)
        {
            var result = await Query<MailMessageInteraction>().Select(MailMessageDetailedModelSelectExpression).FirstOrDefaultAsync(x => x.Id == id);

            if(result != null)
            {
                result.Redirects = await EmailRedirectsQueryWorker.GetCatchesByEmailId(id);
            }

            return result;
        }

        /// <summary>
        /// Получить Email отправленные клиенту
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<MailMessageModel>> GetClientMailMessages(GetClientInteractions model)
        {
            var queryWithStatus = GetQueryWithStatus(Query<MailMessageInteraction>().BuildQuery(model.GetCriterias()));

            return EFCoreExtensions.GetAsync(model, queryWithStatus.OrderByDescending(x => x.Interaction.CreatedOn), MailMessageModelSelectExpression);
        }

        internal static Expression<Func<ApplicationInteractionWithStatus<MailMessageInteraction>, MailMessageModel>> MailMessageModelSelectExpression = x => new MailMessageModel
        {
            Id = x.Interaction.Id,
            Body = x.Interaction.MessageText,
            Header = x.Interaction.TitleText,
            ReadOn = x.Interaction.ReadOn,
            SentOn = x.Interaction.SentOn,
            EmailAddress = x.Interaction.ReceiverEmail,
            Status = x.Status
        };

        public static Expression<Func<MailMessageInteraction, MailMessageDetailedModel>> MailMessageDetailedModelSelectExpression = x => new MailMessageDetailedModel
        {
            Id = x.Id,
            Body = x.MessageText,
            Header = x.TitleText,
            ReadOn = x.ReadOn,
            SentOn = x.SentOn,
            EmailAddress = x.ReceiverEmail,
            Statuses = x.Statuses.OrderByDescending(t => t.StartedOn).Select(t => new InteractionStatusModel
            {
                StartedOn = t.StartedOn,
                Status = t.Status,
                StatusDescription = t.StatusDescription
            }).ToList()
        };
    }
}