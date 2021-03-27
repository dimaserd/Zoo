using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Files;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Files.Abstractions;
using Ecc.Contract.Abstractions;
using Ecc.Contract.Models.Emails;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Microsoft.Extensions.Logging;

namespace Ecc.Logic.Workers.Emails
{
    public class EmailWrapperSender : BaseEccWorker
    {
        IDbFileManager FileManager { get; }
        IEmailSender EmailSender { get; }

        public EmailWrapperSender(ICrocoAmbientContextAccessor ambientContextAccessor, 
            ICrocoApplication application, IDbFileManager fileManager, IEmailSender emailSender) : base(ambientContextAccessor, application)
        {
            FileManager = fileManager;
            EmailSender = emailSender;
        }

        private async Task<(TModel, BaseApiResponse)> Wrapper<TModel>(Func<SendEmailModelWithLoadedAttachments, Task<BaseApiResponse>> sender,  
            TModel model)
            where TModel : ISendEmailModel
        {
            var emailSimpleModel = model.ToSendEmailModel();
            var emailModel = await ToModelWithLoadedAttachments(emailSimpleModel);
            var res = await sender(emailModel);

            return (model, res);
        }

        public async Task<(TModel, BaseApiResponse)[]> SendEmails<TModel>(IEnumerable<TModel> messages)
            where TModel : ISendEmailModel
        {
            try
            {
                var tasks = messages.Select(x => Wrapper(m => EmailSender.SendEmail(m), x));

                var results = await Task.WhenAll(tasks);

                return results.ToArray();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "InnerSmtpEmailSender.SendEmailsInner.OnException");
                Logger.LogWarning("InnerSmtpEmailSender.SendEmails.BeforeError", "Произошла ошибка при инициализации SmtpClient при отправке email");

                var errorResp = new BaseApiResponse(ex);

                return messages.Select(x => (x, errorResp)).ToArray();
            }
        }

        private async Task<SendEmailModelWithLoadedAttachments> ToModelWithLoadedAttachments(SendEmailModel model)
        {
            return new SendEmailModelWithLoadedAttachments
            {
                Body = model.Body,
                Email = model.Email,
                Subject = model.Subject,
                AttachmentFiles = await GetFileDatas(model.AttachmentFileIds)
            };
        }

        private async Task<IFileData[]> GetFileDatas(int[] fileIds)
        {
            if (fileIds == null)
            {
                return null;
            }

            var files = new List<IFileData>();

            foreach (var fileId in fileIds)
            {
                files.Add(await FileManager.GetFileDataById(fileId));
            }

            files = files.Where(x => x != null).ToList();

            return files.ToArray();
        }
    }
}