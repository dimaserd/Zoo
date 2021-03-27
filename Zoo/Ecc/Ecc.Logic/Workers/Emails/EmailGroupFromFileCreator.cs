using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Emails
{
    public class EmailGroupFromFileCreator : BaseEccWorker
    {
        public EmailGroupFromFileCreator(ICrocoAmbientContextAccessor context, 
            ICrocoApplication application) : base(context, application)
        {
        }

        public async Task<BaseApiResponse> ApppendEmailsToGroup(AppendEmailsFromFileToGroup model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (!await Query<EmailGroup>().AnyAsync(x => x.Id == model.EmailGroupId))
            {
                return new BaseApiResponse(false, "Группа не найдена по указанному идентификатору");
            }

            if (!File.Exists(Application.MapPath(model.FilePath)))
            {
                return new BaseApiResponse(false, "Файл не существует");
            }

            //Отложено добавляем
            await PublishMessageAsync(model);
            
            return await TrySaveChangesAndReturnResultAsync($"В группу emailов начали добавляться новые электронные адреса");
        }
    }
}