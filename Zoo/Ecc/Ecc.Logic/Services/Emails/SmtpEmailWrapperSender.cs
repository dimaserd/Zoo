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
using Ecc.Logic.Services.Base;
using Microsoft.Extensions.Logging;

namespace Ecc.Logic.Services.Emails
{
    /// <summary>
    /// Обертка для отправителей эмейлов
    /// </summary>
    public class EmailWrapperSender : BaseEccService
    {
        IDbFileManager FileManager { get; }
        IEmailSender EmailSender { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContextAccessor"></param>
        /// <param name="application"></param>
        /// <param name="fileManager"></param>
        /// <param name="emailSender"></param>
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

        /// <summary>
        /// Отправить эмейлы
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="messages"></param>
        /// <returns></returns>
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