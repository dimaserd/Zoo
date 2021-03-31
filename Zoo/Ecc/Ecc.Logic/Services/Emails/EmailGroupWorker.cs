using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Services.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Services.Emails
{
    /// <summary>
    /// Сервис для работы с группами электронных адресов
    /// </summary>
    public class EmailGroupWorker : BaseEccService
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        public EmailGroupWorker(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application) : base(ambientContext, application)
        {
        }

        /// <summary>
        /// Удалить группу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> RemoveGroup(string id)
        {
            if (!IsUserAdmin())
            {
                return new BaseApiResponse(false, "У вас недостаточно прав");
            }

            var emailGroup = await Query<EmailGroup>().FirstOrDefaultAsync(x => x.Id == id);
            
            if(emailGroup == null)
            {
                return new BaseApiResponse(false, "Группа не найдена по указанному идентификатору");
            }

            DeleteHandled(emailGroup);

            return await TrySaveChangesAndReturnResultAsync("Группа для эмелов удалена");
        }

        /// <summary>
        /// Создать группу
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<string>> CreateGroup(CreateEmailGroup model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if(!validation.IsSucceeded)
            {
                return new BaseApiResponse<string>(validation);
            }

            var repo = GetRepository<EmailGroup>();

            if(await repo.Query().AnyAsync(x => x.Name == model.Name))
            {
                return new BaseApiResponse<string>(false, $"Группа эмейлов с именем '{model.Name}' уже создана");
            }

            var id = Guid.NewGuid().ToString();

            repo.CreateHandled(new EmailGroup
            {
                Id = id,
                Name = model.Name
            });

            return await TrySaveChangesAndReturnResultAsync("Группа эмейлов создана", id);
        }

        /// <summary>
        /// Добавить адресов в группу
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> AddEmailsToGroup(AddEmailsToEmailGroup model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var emailGroup = await Query<EmailGroup>().Include(x => x.Emails).FirstOrDefaultAsync(x => x.Id == model.EmailGroupId);

            if (emailGroup == null)
            {
                return new BaseApiResponse(false, "Группа для эмелов не найдена по указанному идентификатору");
            }

            var emailsInGroup = emailGroup.Emails.Select(x => x.Email).ToDictionary(x => x);

            var emailsToAdd = model.Emails.Where(x => !emailsInGroup.ContainsKey(x)).Select(x => new EmailInEmailGroupRelation
            {
                Id = Guid.NewGuid().ToString(),
                EmailGroupId = emailGroup.Id,
                Email = x
            }).ToList();

            CreateHandled(emailsToAdd);

            return await TrySaveChangesAndReturnResultAsync("Электронные адреса добавлены в группу");
        }
    }
}