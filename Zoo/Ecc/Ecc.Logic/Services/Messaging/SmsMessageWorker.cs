using Ecc.Logic.Resources;
using Ecc.Logic.Models.Messaging;
using Ecc.Model.Entities.External;
using Ecc.Model.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecc.Model.Consts;
using Croco.Core.Search.Extensions;
using Ecc.Contract.Models.Sms;
using System.Linq.Expressions;
using Ecc.Logic.Models;
using Ecc.Common.Enumerations;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;

namespace Ecc.Logic.Services.Messaging
{
    public class SmsMessageWorker : ApplicationInteractionWorker
    {
        public SmsMessageWorker(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application) : base(ambientContext, application)
        {
        }

        public static readonly Expression<Func<ApplicationInteractionWithStatus<SmsMessageInteraction>, SmsMessageModel>> SelectExpression = x => new SmsMessageModel
        {
            Id = x.Interaction.Id,
            Body = x.Interaction.MessageText,
            Header = x.Interaction.TitleText,
            ReadOn = x.Interaction.ReadOn,
            SentOn = x.Interaction.SentOn,
            PhoneNumber = x.Interaction.PhoneNumber,
            Status = x.Status
        };

        public async Task<BaseApiResponse> SendSms(SendSmsToClient model)
        {
            if(!IsUserAdmin())
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAnAdministrator);
            }

            var phoneAndId = await Query<EccUser>().Select(x => new { x.PhoneNumber, x.Id }).FirstOrDefaultAsync(x => x.Id == model.ClientId);

            if (phoneAndId == null)
            {
                return new BaseApiResponse(false, "Клиент не найден по указанному идентификатору");
            }

            var toMes = ToSmsMessage(model, phoneAndId.PhoneNumber);

            GetRepository<SmsMessageInteraction>().CreateHandled(toMes.Item1);
            GetRepository<InteractionStatusLog>().CreateHandled(toMes.Item2);

            return await TrySaveChangesAndReturnResultAsync("Sms-сообщение добавлено в очередь");
        }

        public Task<GetListResult<SmsMessageModel>> GetClientSmsMessages(GetClientInteractions model)
        {
            var initQuery = Query<SmsMessageInteraction>();

            if(!string.IsNullOrWhiteSpace(model.ClientId))
            {
                initQuery = initQuery.Where(x => x.UserId == model.ClientId);
            }

            return EFCoreExtensions.GetAsync(model, GetQueryWithStatus(initQuery).OrderByDescending(x => x.Interaction.CreatedOn), SelectExpression);
        }

        public (SmsMessageInteraction, InteractionStatusLog, List<InteractionAttachment>) ToSmsMessage(SendSmsToClient message, string phoneNumber)
        {
            var id = Guid.NewGuid().ToString();

            var attachments = message.AttachmentFileIds?.Select(x => new InteractionAttachment
            {
                FileId = x,
                InteractionId = id,
            }).ToList();

            return (new SmsMessageInteraction
            {
                Id = id,
                TitleText = null,
                MessageText = message.Message,
                SendNow = true,
                UserId = message.ClientId,
                Type = EccConsts.SmsType,
                Attachments = attachments,
                PhoneNumber = phoneNumber,
            },
            new InteractionStatusLog
            {
                InteractionId = id,
                Status = InteractionStatus.Created,
                StartedOn = Application.DateTimeProvider.Now
            },
            attachments);
        }
    }
}