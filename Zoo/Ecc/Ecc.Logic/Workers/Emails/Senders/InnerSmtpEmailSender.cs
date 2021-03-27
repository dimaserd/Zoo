using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Files;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Files.Abstractions;
using Ecc.Contract.Models.Emails;
using Ecc.Contract.Settings;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Microsoft.Extensions.Logging;

namespace Ecc.Logic.Workers.Emails
{
    public class InnerSmtpEmailSender : BaseEccWorker, IEmailSender
    {
        SmtpEmailSettingsModel EmailSettings { get; }
        IDbFileManager FileManager { get; }

        public InnerSmtpEmailSender(ICrocoAmbientContextAccessor ambientContextAccessor, 
            ICrocoApplication application, IDbFileManager fileManager) : base(ambientContextAccessor, application)
        {
            EmailSettings = Application.SettingsFactory.GetSetting<SmtpEmailSettingsModel>();
            FileManager = fileManager;
        }


        public Task<List<(TModel, BaseApiResponse)>> SendEmails<TModel>(IEnumerable<TModel> messages, Func<TModel, SendEmailModel> mapper)
        {
            return SendEmailsInner(messages, mapper);
        }

        private async Task<List<(TModel, BaseApiResponse)>> SendEmailsInner<TModel>(IEnumerable<TModel> messages, Func<TModel, SendEmailModel> mapper)
        {
            try
            {
                using var smtpMail = GetSmtpClient();

                var tasks = messages.Select(x => SendSingleEmail(smtpMail, x, mapper(x)));

                var results = await Task.WhenAll(tasks);

                return results.ToList();
            }
            catch (Exception ex)
            {                
                Logger.LogError(ex, "InnerSmtpEmailSender.SendEmailsInner.OnException");
                Logger.LogWarning("InnerSmtpEmailSender.SendEmails.BeforeError", "Произошла ошибка при инициализации SmtpClient при отправке email");

                var errorResp = new BaseApiResponse(ex);

                return messages.Select(x => (x, errorResp)).ToList();
            }
        }


        private SmtpClient GetSmtpClient() => new SmtpClient(EmailSettings.SmtpClientString, EmailSettings.SmtpPort)
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(EmailSettings.UserName, EmailSettings.Password),
        };
        

        private async Task<(TModel, BaseApiResponse)> SendSingleEmail<TModel>(SmtpClient smtpClient, TModel model, SendEmailModel emailModel)
        {
            using (var mail = new MailMessage(new MailAddress(EmailSettings.FromAddress), new MailAddress(emailModel.Email))
            {
                Subject = emailModel.Subject,
                Body = emailModel.Body,
                IsBodyHtml = EmailSettings.IsBodyHtml
            })
            {
                try
                {
                    //Добавляем вложения в письмо
                    AddAttachments(mail, await GetFileDatas(emailModel.AttachmentFileIds));
                    //отправляем письмо
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "InnerSmtpEmailSender.SendSingleEmail.Exception");
                    Logger.LogWarning("InnerSmtpEmailSender.SendSingleEmail.Exception", "Произошла ошибка при отправке emzil сообщения через SmtpClient");
                    return (model, new BaseApiResponse(ex));
                }
            }

            return (model, new BaseApiResponse(true, "Ok"));
        }

        private async Task<IFileData[]> GetFileDatas(List<int> fileIds)
        {
            if (fileIds == null)
            {
                return null;
            }

            var files = new List<IFileData>();

            foreach(var fileId in fileIds)
            {
                files.Add(await FileManager.GetFileDataById(fileId));
            }

            files = files.Where(x => x != null).ToList();

            return files.ToArray();
        }

        private void AddAttachments(MailMessage mail, IFileData[] attachments)
        {
            if (attachments == null)
            {
                return;
            }

            foreach (var attachment in attachments)
            {
                mail.Attachments.Add(GetEmailAttachment(attachment));
            }
        }

        private Attachment GetEmailAttachment(IFileData fileData)
        {
            var ms = new MemoryStream(fileData.FileData) { Position = 0 };

            return new Attachment(ms, Path.GetFileName(fileData.FileName));
        }
    }
}