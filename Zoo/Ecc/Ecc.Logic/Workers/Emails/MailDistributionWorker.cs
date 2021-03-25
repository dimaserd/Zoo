using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ecc.Contract.Models.MailDistributions;
using Ecc.Logic.Models.Messaging;
using Ecc.Model.Entities.Ecc.Messaging;
using Ecc.Model.Entities.External;
using Ecc.Logic.Workers.Base;
using Ecc.Logic.Resources;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;

namespace Ecc.Logic.Workers.Emails
{
    public class MailDistributionWorker : BaseEccWorker
    {
        public async Task<BaseApiResponse> CreateAsync(MailDistributionCreate model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if(!validation.IsSucceeded)
            {
                return validation;
            }
            
            CreateHandled(new MailDistribution
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Body = model.Body,
                SendToEveryUser = model.SendToEveryUser,
                Subject = model.Subject
            });

            return await TrySaveChangesAndReturnResultAsync($"Рассылка {model.Name} создана");
        }

        public async Task<BaseApiResponse> EditMailDistributionAsync(MailDistributionEdit model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if(!validation.IsSucceeded)
            {
                return validation;
            }

            var repo = GetRepository<MailDistribution>();

            var mailDistribution = await repo.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (mailDistribution == null)
            {
                return new BaseApiResponse(false, "Рассылки не существует");
            }

            mailDistribution.Name = model.Name;
            mailDistribution.Body = model.Body;
            mailDistribution.SendToEveryUser = model.SendToEveryUser;
            mailDistribution.Subject = model.Subject;

            repo.UpdateHandled(mailDistribution);

            return await TrySaveChangesAndReturnResultAsync("Рассылка отредактирована");
        }

        public async Task<BaseApiResponse> RemoveUserGroupFromMailDistributionAsync(MainDistributionUserGroupRelationIdModel model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if(!validation.IsSucceeded)
            {
                return validation;
            }

            if (!await Query<MailDistribution>().AnyAsync(x => x.Id == model.MailDistributionId))
            {
                return new BaseApiResponse(false, "Рассылка не найдена по указанному идентификатору");
            }

            if (!await Query<EccUserGroup>().AnyAsync(x => x.Id == model.GroupId))
            {
                return new BaseApiResponse(false, "Группа пользователей не найдена по указанному идентификатору");
            }

            var mailRelationRepo = GetRepository<MailDistributionUserGroupRelation>();

            var elem = await mailRelationRepo.Query()
                .FirstOrDefaultAsync(x => x.MailDistributionId == model.MailDistributionId && x.GroupId == model.GroupId);

            if (elem == null)
            {
                return new BaseApiResponse(false, "Группа пользователей не принадлежит к данной рассылке");
            }

            mailRelationRepo.DeleteHandled(elem);

            return await TrySaveChangesAndReturnResultAsync("Группа пользователей удалена из рассылки");
        }

        public async Task<BaseApiResponse> RemoveMailDistributionAsync(string id)
        {
            if (!IsUserAdmin())
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAnAdministrator);
            }

            var repo = GetRepository<MailDistribution>();

            var elem = await repo.Query()
                .Include(x => x.UserGroups)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (elem == null)
            {
                return new BaseApiResponse(false, "Рассылка не найдена по указанному идентификатору");
            }

            repo.DeleteHandled(elem);
            GetRepository<MailDistributionUserGroupRelation>().DeleteHandled(elem.UserGroups);

            return await TrySaveChangesAndReturnResultAsync("Рассылка удалена");
        }

        public async Task<BaseApiResponse> AddUserGroupToMailDistributionAsync(AddMailDistributionUserGroupRelation model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var repo = GetRepository<MailDistribution>();

            if (!await repo.Query().AnyAsync(x => x.Id == model.MailDistributionId))
            {
                return new BaseApiResponse(false, "Рассылка не найдена по указанному идентификатору");
            }

            if (!await Query<EccUserGroup>().AnyAsync(x => x.Id == model.UserGroupId))
            {
                return new BaseApiResponse(false, "Группа пользователей не найден по указанному идентификатору");
            }

            var mailRelationRepo = GetRepository<MailDistributionUserGroupRelation>();

            if (await mailRelationRepo.Query().AnyAsync(x => x.MailDistributionId == model.MailDistributionId && x.GroupId == model.UserGroupId))
            {
                return new BaseApiResponse(false, "Группа пользователей уже принадлежит данной рассылке");
            }

            MailDistributionUserGroupRelation mailDistributionUserGroupRelation = new MailDistributionUserGroupRelation
            {
                MailDistributionId = model.MailDistributionId,
                GroupId = model.UserGroupId,
            };

            mailRelationRepo.CreateHandled(mailDistributionUserGroupRelation);

            return await TrySaveChangesAndReturnResultAsync("Группа пользователей добавлена к рассылке");
        }

        public Task<List<MailDistributionModel>> GetMailDistributionsAsync()
        {
            return Query<MailDistribution>().Select(MailDistributionModel.SelectExpression).ToListAsync();
        }

        public Task<MailDistributionModel> GetMailDistributionWithUserGroupsAsync(string id)
        {
            return Query<MailDistribution>().Select(MailDistributionModel.SelectExpression).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BaseApiResponse> ChangeUsersInGroupAsync(ChangeUserGroupsInMailDistributionModel model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (model.UserGroupActions == null || model.UserGroupActions.Count == 0)
            {
                return new BaseApiResponse(false, "Вы подали пустой массив изменений");
            }

            var repo = GetRepository<MailDistribution>();

            var mailDistribution = await repo.Query().Include(x => x.UserGroups).FirstOrDefaultAsync(x => x.Id == model.MailDistributionId);

            if (mailDistribution == null)
            {
                return new BaseApiResponse(false, "Рассылка не найдена по указанному идентификатору");
            }

            var set = GetRepository<MailDistributionUserGroupRelation>();

            foreach (var userGroupToDelete in model.UserGroupActions.Where(x => !x.AddOrDelete))
            {
                var user = mailDistribution.UserGroups.FirstOrDefault(x => x.GroupId == userGroupToDelete.UserGroupId);

                set.DeleteHandled(user);
            }

            foreach (var userGroupToAddModel in model.UserGroupActions.Where(x => x.AddOrDelete))
            {
                MailDistributionUserGroupRelation mailDistributionUserGroupRelation = new MailDistributionUserGroupRelation
                {
                    MailDistributionId = model.MailDistributionId,
                    GroupId = userGroupToAddModel.UserGroupId,
                };

                set.CreateHandled(mailDistributionUserGroupRelation);
            }

            return await TrySaveChangesAndReturnResultAsync("Группы пользователей изменены");
        }

        public MailDistributionWorker(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application) : base(ambientContext, application)
        {
        }
    }
}