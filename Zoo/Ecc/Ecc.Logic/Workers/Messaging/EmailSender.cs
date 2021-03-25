﻿using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Core.Workers;
using Ecc.Logic.Extensions;
using Ecc.Logic.Workers.Base;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Messaging
{
    public class EmailSender : BaseEccWorker
    {
        IEccFilePathMapper FilePathMapper { get; }
        EmailDelayedSender EmailDelayedSender { get; }

        public EmailSender(ICrocoAmbientContextAccessor ambientContext, 
            ICrocoApplication application, 
            IEccFilePathMapper filePathMapper, 
            EmailDelayedSender emailDelayedSender) : base(ambientContext, application)
        {
            FilePathMapper = filePathMapper;
            EmailDelayedSender = emailDelayedSender;
        }

        
        public async Task<BaseApiResponse> SendEmailViaTemplate(SendMailMessageViaHtmlTemplate model)
        {
            var sendModel = model.ToSendEmailModel(FilePathMapper);

            if (!sendModel.IsSucceeded)
            {
                return new BaseApiResponse(sendModel);
            }

            var resp = sendModel.ResponseObject;

            return await EmailDelayedSender.SendEmail(new SendMailMessage
            {
                Body = resp.Body,
                Email = resp.Email,
                AttachmentFileIds = resp.AttachmentFileIds,
                Subject = resp.Subject,
            });
        }
    }
}