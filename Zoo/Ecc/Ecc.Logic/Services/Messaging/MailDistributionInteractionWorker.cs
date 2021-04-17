using Ecc.Contract.Models.Emails;
using Ecc.Logic.Services.Emails;
using Ecc.Model.Entities.Interactions;
using Ecc.Common.Enumerations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ecc.Contract.Models;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;

namespace Ecc.Logic.Services.Messaging
{
    /// <summary>
    /// Сервис для работы с рассылками электронной почты
    /// </summary>
    public class MailDistributionInteractionWorker : ApplicationInteractionWorker
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        MailMessageSender MailMessageSender { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        /// <param name="mailMessageSender"></param>
        public MailDistributionInteractionWorker(ICrocoAmbientContextAccessor ambientContext, 
            ICrocoApplication application,
            MailMessageSender mailMessageSender) : base(ambientContext, application)
        {
            MailMessageSender = mailMessageSender;
        }

        /// <summary>
        /// Отправить эмейлы
        /// </summary>
        /// <returns></returns>
        public async Task SendEmailsAsync()
        {
            await semaphore.WaitAsync();

            var interactions = await GetQueryWithStatus(Query<MailMessageInteraction>())
                .Where(x => x.Status == InteractionStatus.Created)
                .Select(x => new SendEmailModelWithInteractionId
                {
                    InteractionId = x.Interaction.Id,
                    EmailModel = new SendEmailModel
                    {
                        Subject = x.Interaction.TitleText,
                        Body = x.Interaction.MessageText,
                        Email = x.Interaction.ReceiverEmail,
                        AttachmentFileIds = x.Interaction.Attachments.Select(t => t.FileId).ToArray(),
                    }
                })
                .ToListAsync();

            await SetStatusForInteractions(interactions.Select(x => x.InteractionId), InteractionStatus.InProccess, "In process of sending emails");

            var updates = await MailMessageSender.SendInteractions(interactions);

            await UpdateInteractionStatusesAsync(updates);

            semaphore.Release();
        }
    }
}