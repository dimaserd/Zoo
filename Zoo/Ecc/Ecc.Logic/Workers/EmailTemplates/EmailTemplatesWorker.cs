using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Ecc.Logic.Models.EmailTemplates;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;

namespace Ecc.Logic.Workers.EmailTemplates
{
    public class EmailTemplatesWorker : BaseEccWorker
    {
        public async Task<BaseApiResponse> CreateEmailTemplateAsync(CreateEmailTemplate model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            CreateHandled(new EmailTemplate
            {
                CustomEmailType = model.CustomEmailType,
                IsActive = model.IsActive,
                IsJsScripted = model.IsJsScripted,
                JsScript = model.JsScript,
                TemplateType = model.TemplateType
            });

            return await TrySaveChangesAndReturnResultAsync("Шаблон создан");
        }

        public async Task<BaseApiResponse> UpdateEmailTemplateAsync(EditEmailTemplate model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var repository = GetRepository<EmailTemplate>();

            var emailTemplate = await repository.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

            if(emailTemplate == null)
            {
                return new BaseApiResponse(false, "Шаблон не найден по указанному идентификатору");
            }

            emailTemplate.CustomEmailType = model.CustomEmailType;
            emailTemplate.IsActive = model.IsActive;
            emailTemplate.IsJsScripted = model.IsJsScripted;
            emailTemplate.JsScript = model.JsScript;
            emailTemplate.TemplateType = model.TemplateType;

            repository.UpdateHandled(emailTemplate);

            return await TrySaveChangesAndReturnResultAsync("Шаблон обновлён");
        }

        public Task<List<EmailTemplateModel>> GetEmailTemplatesAsync()
        {
            return Query<EmailTemplate>().Select(x => new EmailTemplateModel
            {
                Id = x.Id,
                IsActive = x.IsActive,
                CustomEmailType = x.CustomEmailType,
                IsJsScripted = x.IsJsScripted,
                JsScript = x.JsScript,
                TemplateType = x.TemplateType
            }).ToListAsync();
        }

        public Task<EmailTemplateModel[]> GetActiveEmailTemplates(string templateType)
        {
            return Query<EmailTemplate>()
                .Where(x => x.IsActive && x.TemplateType == templateType)
                .Select(x => new EmailTemplateModel
                {
                    IsJsScripted = x.IsJsScripted,
                    JsScript = x.JsScript,
                    TemplateType = x.TemplateType,
                    CustomEmailType = x.CustomEmailType,
                    Id = x.Id,
                    IsActive = x.IsActive
                }).ToArrayAsync();
        }

        public EmailTemplatesWorker(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application) : base(ambientContext, application)
        {
        }
    }
}