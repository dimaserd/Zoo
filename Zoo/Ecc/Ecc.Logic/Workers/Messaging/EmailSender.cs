using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models;
using Ecc.Logic.Core.Workers;
using Ecc.Logic.Extensions;
using Ecc.Logic.Workers.Base;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Messaging
{
    /// <summary>
    /// Отправитель электроннной почты
    /// </summary>
    public class EmailSender : BaseEccWorker
    {
        EmailDelayedSender EmailDelayedSender { get; }

        /// <summary>
        /// Отправитель email
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        /// <param name="emailDelayedSender"></param>
        public EmailSender(ICrocoAmbientContextAccessor ambientContext, 
            ICrocoApplication application, 
            EmailDelayedSender emailDelayedSender) : base(ambientContext, application)
        {
            EmailDelayedSender = emailDelayedSender;
        }
        
        /// <summary>
        /// Отправить через шаблон
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> SendEmailViaTemplate(SendMailMessageViaHtmlTemplate model)
        {
            var sendModel = model.ToSendEmailModel(Application.MapPath);

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