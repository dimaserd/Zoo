using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Extensions;
using Ecc.Logic.Models;
using Ecc.Logic.Services.Base;
using Ecc.Logic.Settings;
using Ecc.Model.Entities.LinkCatch;
using System;
using System.Linq;

namespace Ecc.Logic.Services.Emails
{
    /// <summary>
    /// Сервис для процессинга текста
    /// </summary>
    public class EmailLinkFunctionInvoker : BaseEccService
    {
        string UrlRedirectFormat { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContextAccessor"></param>
        /// <param name="application"></param>
        /// <param name="settings"></param>
        public EmailLinkFunctionInvoker(ICrocoAmbientContextAccessor ambientContextAccessor,
            ICrocoApplication application, EccSettings settings) : base(ambientContextAccessor, application)
        {
            UrlRedirectFormat = settings.FunctionInvokerSettings.UrlRedirectFormat;
        }

        private string GetUrlById(string id)
        {
            return string.Format(UrlRedirectFormat, id);
        }

        /// <summary>
        /// Преобразовать текст
        /// </summary>
        /// <param name="interactionId"></param>
        /// <param name="replacing"></param>
        /// <returns></returns>
        public string ProccessText(string interactionId, EccReplacing replacing)
        {
            var firstArg = replacing.Func.Args.FirstOrDefault();

            if (firstArg == null)
            {
                return replacing.TextToReplace;
            }

            var id = Guid.NewGuid().ToString();

            var linkCatch = new EmailLinkCatch
            {
                Id = id,
                Url = firstArg,
                CreatedOnUtc = DateTime.UtcNow,
                MailMessageId = interactionId
            };

            RepositoryFactory.CreateHandled(linkCatch);

            return GetUrlById(id);
        }
    }
}