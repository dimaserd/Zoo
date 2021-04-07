using Croco.Core.Contract.Application;
using Croco.Core.EventSourcing.Implementations;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Services;
using Ecc.Model.Contexts;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppendEmailsFromFileToGroupMessageHandler : CrocoMessageHandler<AppendEmailsFromFileToGroup>
    {
        const int CountInPack = 100;

        EmailListExtractor EmailsExtractor { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="emailsExtractor"></param>
        /// <param name="application"></param>
        /// <param name="logger"></param>
        public AppendEmailsFromFileToGroupMessageHandler(EmailListExtractor emailsExtractor,
            ICrocoApplication application, 
            ILogger<AppendEmailsFromFileToGroupMessageHandler> logger) : base(application, logger)
        {
            EmailsExtractor = emailsExtractor;
        }

        /// <summary>
        /// Обработать сообщение
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task HandleMessage(AppendEmailsFromFileToGroup model)
        {
            var emailsSafeValue = await GetSystemTransactionHandler().ExecuteAndCloseTransactionSafe(amb =>
            {
                return amb.GetAmbientContext<EccDbContext>().DataConnection
                    .GetRepositoryFactory()
                    .Query<EmailInEmailGroupRelation>()
                    .Where(x => x.EmailGroupId == model.EmailGroupId)
                    .Select(x => x.Email)
                    .ToListAsync();
            });

            if (!emailsSafeValue.IsSucceeded)
            {
                throw new Exception("Ошибка");
            }

            var emailsDict = emailsSafeValue.Value.ToDictionary(x => x);

            var emailsFromFileResult = await EmailsExtractor.ExtractEmailsListFromFile(model.FilePath);

            if (!emailsFromFileResult.IsSucceeded)
            {
                throw new Exception(emailsFromFileResult.Message);
            }

            var newEmails = emailsFromFileResult.ResponseObject.Where(x => !emailsDict.ContainsKey(x)).ToList();

            var count = 0;

            while (count < newEmails.Count)
            {
                await GetSystemTransactionHandler().ExecuteAndCloseTransactionSafe(amb =>
                {
                    var repoFactory = amb
                        .GetAmbientContext<EccDbContext>()
                        .DataConnection
                        .GetRepositoryFactory();

                    var repo = repoFactory
                        .GetRepository<EmailInEmailGroupRelation>();

                    var emailsToAdd = newEmails.Skip(count).Take(CountInPack).Select(x => new EmailInEmailGroupRelation 
                    {
                        EmailGroupId = model.EmailGroupId,
                        Email = x
                    }).ToList();

                    repo.CreateHandled(emailsToAdd);

                    return repoFactory.SaveChangesAsync();
                });

                count += CountInPack;
            }
        }
    }
}