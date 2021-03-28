using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Files;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models.Emails;
using Ecc.Contract.Settings;
using Ecc.Logic.Abstractions;
using Microsoft.Extensions.Logging;

namespace Ecc.Logic.Services.Emails.Senders
{
    /// <summary>
    /// Отправитель Email по smtp
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {
        SmtpEmailSettingsModel EmailSettings { get; }
        SmtpClient SmtpClient { get; }
        ILogger Logger { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="application"></param>
        /// <param name="logger"></param>
        public SmtpEmailSender(ICrocoApplication application, ILogger<SmtpEmailSender> logger)
        {
            EmailSettings = application.SettingsFactory.GetSetting<SmtpEmailSettingsModel>();
            SmtpClient = GetSmtpClient();
            Logger = logger;
        }

        /// <summary>
        /// Отправить email
        /// </summary>
        /// <param name="emailModel"></param>
        /// <returns></returns>
        public Task<BaseApiResponse> SendEmail(SendEmailModelWithLoadedAttachments emailModel)
        {
            using var mail = ToMailMessage(emailModel);

            try
            {
                //отправляем письмо
                SmtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "InnerSmtpEmailSender.SendSingleEmail.Exception");
                Logger.LogWarning("InnerSmtpEmailSender.SendSingleEmail.Exception", "Произошла ошибка при отправке emzil сообщения через SmtpClient");
                return Task.FromResult(new BaseApiResponse(ex));
            }

            return Task.FromResult(new BaseApiResponse(true, "Ok"));
        }

        private SmtpClient GetSmtpClient() => new SmtpClient(EmailSettings.SmtpClientString, EmailSettings.SmtpPort)
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(EmailSettings.UserName, EmailSettings.Password),
        };


        private MailMessage ToMailMessage(SendEmailModelWithLoadedAttachments model)
        {
            var result = new MailMessage(new MailAddress(EmailSettings.FromAddress), new MailAddress(model.Email))
            {
                Subject = model.Subject,
                Body = model.Body,
                IsBodyHtml = EmailSettings.IsBodyHtml
            };

            AddAttachments(result, model.AttachmentFiles);

            return result;
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