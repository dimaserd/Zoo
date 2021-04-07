using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecc.Contract.Models.Interactions;
using Ecc.Common.Enumerations;
using Ecc.Contract.Models;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Ecc.Logic.Services.Base;

namespace Ecc.Logic.Services.Emails
{
    /// <summary>
    /// Класс посылающий письма
    /// </summary>
    public class MailMessageSender : BaseEccService
    {
        EmailWrapperSender EmailSender { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        /// <param name="emailSender"></param>
        public MailMessageSender(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application, EmailWrapperSender emailSender) : base(ambientContext, application)
        {
            EmailSender = emailSender;
        }

        /// <summary>
        /// Отправить взаимодействия
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public async Task<List<UpdateInteractionStatus>> SendInteractions(List<SendEmailModelWithInteractionId> messages)
        {
            var fileIds = messages.SelectMany(x => x.EmailModel.AttachmentFileIds).ToArray();

            var res = await EmailSender.SendEmails(messages);

            return res.Select(x => new UpdateInteractionStatus
            {
                Id = x.Item1.InteractionId,
                Status = x.Item2.IsSucceeded ? InteractionStatus.Sent : InteractionStatus.Error,
                StatusDescription = x.Item2.IsSucceeded ? null : x.Item2.Message
            }).ToList();
        }
    }
}