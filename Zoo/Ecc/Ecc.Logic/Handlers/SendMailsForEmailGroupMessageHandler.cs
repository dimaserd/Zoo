using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.EventSourcing.Implementations;
using Croco.Core.Utils;
using Ecc.Contract.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Core.Workers;
using Ecc.Model.Contexts;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    public class SendMailsForEmailGroupMessageHandler : CrocoMessageHandler<SendMailsForEmailGroup>
    {
        EmailDelayedSender EmailDelayedSender { get; }

        const int CountInPack = 100;

        public SendMailsForEmailGroupMessageHandler(ICrocoApplication application,
            EmailDelayedSender emailDelayedSender,
            ILogger<SendMailsForEmailGroupMessageHandler> logger) : base(application, logger)
        {
            EmailDelayedSender = emailDelayedSender;
        }


        public async Task<BaseApiResponse> StartEmailDistribution(SendMailsForEmailGroup model)
        {
            var eamilsInGroupSafeValue = await GetSystemTransactionHandler().ExecuteAndCloseTransactionSafe(amb =>
            {
                IQueryable<EmailInEmailGroupRelation> initQuery = 
                    amb.GetAmbientContext<EccDbContext>()
                    .DataConnection.GetRepositoryFactory()
                    .Query<EmailInEmailGroupRelation>()
                    .Where(x => x.EmailGroupId == model.EmailGroupId)
                    .OrderBy(x => x.Email);
                
                return initQuery.Select(x => x.Email).ToListAsync();
            });
                
            if (!eamilsInGroupSafeValue.IsSucceeded)
            {
                throw new Exception("Не удалось получить список эмейлов из группы");
            }

            var emails = eamilsInGroupSafeValue.Value;

            var count = 0;

            var messageDistribution = Guid.NewGuid().ToString();

            await GetSystemTransactionHandler().ExecuteAndCloseTransactionSafe(amb =>
            {
                var repoFactory = amb.GetAmbientContext<EccDbContext>()
                    .DataConnection.GetRepositoryFactory();

                repoFactory.GetRepository<MessageDistribution>().CreateHandled(new MessageDistribution
                {
                    Id = messageDistribution,
                    Type = "SendMailsByEmailGroup",
                    Data = Tool.JsonConverter.Serialize(model)
                });

                return repoFactory.SaveChangesAsync();
            });

            while (count < emails.Count)
            {
                await EmailDelayedSender.SendEmails(emails.Skip(count).Take(CountInPack).Select(x => new SendMailMessage
                {
                    Email = x,
                    Body = model.Body,
                    Subject = model.Subject,
                    MessageDistributionId = messageDistribution,
                    AttachmentFileIds = model.AttachmentFileIds
                }));

                count += CountInPack;
            }

            return new BaseApiResponse(true, "Ok");
        }

        public override Task HandleMessage(SendMailsForEmailGroup model)
        {
            return StartEmailDistribution(model);
        }
    }
}