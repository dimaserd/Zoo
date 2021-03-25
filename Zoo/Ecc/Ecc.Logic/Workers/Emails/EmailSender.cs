using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecc.Contract.Models.Emails;
using Ecc.Logic.Abstractions;
using Ecc.Contract.Models.Interactions;
using Ecc.Common.Enumerations;
using Ecc.Contract.Models;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Ecc.Logic.Workers.Base;

namespace Ecc.Logic.Workers.Emails
{
    /// <summary>
    /// Класс посылающий письма
    /// </summary>
    public class SmtpEmailSender : BaseEccWorker
    {
        IEccFileService FileService { get; }
        IEmailSenderProvider SenderProvider { get; }

        public SmtpEmailSender(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application, 
            IEccFileService fileService, IEmailSenderProvider senderProvider) : base(ambientContext, application)
        {
            FileService = fileService;
            SenderProvider = senderProvider;
        }

        public async Task<List<UpdateInteractionStatus>> SendInteractions(List<SendEmailModelWithInteractionId> messages)
        {
            var fileIds = messages.SelectMany(x => x.EmailModel.AttachmentFileIds).ToArray();

            var sender = SenderProvider.GetEmailSender(new GetEmailSenderOptions 
            {
                AmbientContextAccessor = AmbientContextAccessor,
                //Устанавливаю вложения, получая их из базы данных
                Attachments = await FileService.GetFileDatas(fileIds)
            });

            var res = await sender.SendEmails(messages, x => x.EmailModel);

            return res.Select(x => new UpdateInteractionStatus
            {
                Id = x.Item1.InteractionId,
                Status = x.Item2.IsSucceeded ? InteractionStatus.Sent : InteractionStatus.Error,
                StatusDescription = x.Item2.IsSucceeded ? null : x.Item2.Message
            }).ToList();
        }
    }
}