using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Extensions;
using Ecc.Logic.Models;
using Ecc.Logic.Settings;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.LinkCatch;
using System;
using System.Linq;

namespace Ecc.Logic.Workers.Emails
{
    public class EmailLinkFunctionInvoker : BaseEccWorker
    {
        string UrlRedirectFormat { get; }

        public EmailLinkFunctionInvoker(ICrocoAmbientContextAccessor ambientContextAccessor,
            ICrocoApplication application, EccSettings settings) : base(ambientContextAccessor, application)
        {
            UrlRedirectFormat = settings.FunctionInvokerSettings.UrlRedirectFormat;
        }

        public string GetUrlById(string id)
        {
            return string.Format(UrlRedirectFormat, id);
        }

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