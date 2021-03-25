using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Extensions;
using Ecc.Logic.Settings;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.LinkCatch;
using System;
using System.Linq;

namespace Ecc.Logic.Services
{
    public class EccLinkFunctionInvoker : BaseEccWorker, IEccTextFunctionInvoker
    {
        string UrlRedirectFormat { get; }

        public EccLinkFunctionInvoker(ICrocoAmbientContextAccessor ambientContextAccessor, 
            ICrocoApplication application, EccLinkFunctionInvokerSettings settings) : base(ambientContextAccessor, application)
        {
            UrlRedirectFormat = settings.UrlRedirectFormat;
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