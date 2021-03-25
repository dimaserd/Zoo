using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models.UserGroups;
using Ecc.Logic.Models.Users;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.External;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers
{
    public class EccUserGroupWorker : BaseEccWorker
    {
        public EccUserGroupWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }


        public async Task<BaseApiResponse> CreateGroupAsync(UserGroupCreate model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var userGroup = new EccUserGroup
            {
                Name = model.Name,
                Description = model.Description
            };

            var repo = GetRepository<EccUserGroup>();

            repo.CreateHandled(userGroup);

            return await TrySaveChangesAndReturnResultAsync($"Группа пользователей {model.Name} создана");
        }

        public async Task<BaseApiResponse> EditGroupAsync(UserGroupEdit model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var repo = GetRepository<EccUserGroup>();

            var userGroup = await repo.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (userGroup == null)
            {
                return new BaseApiResponse(false, "Данной группы не существует");
            }

            userGroup.Name = model.Name;
            userGroup.Description = model.Description;

            repo.UpdateHandled(userGroup);

            return await TrySaveChangesAndReturnResultAsync("Группа пользователей отредактирована");
        }

        public async Task<BaseApiResponse> RemoveUserFromGroupAsync(UserInUserGroupIdModel model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var repo = GetRepository<EccUserGroup>();

            var group = await repo.Query()
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == model.UserGroupId);

            if (group == null)
            {
                return new BaseApiResponse(false, "Группа пользователей не создана");
            }

            if (!await Query<EccUser>().AnyAsync(x => x.Id == model.UserId))
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            var elem = group.Users.FirstOrDefault(x => x.UserGroupId == model.UserGroupId && x.UserId == model.UserId);

            if (elem == null)
            {
                return new BaseApiResponse(false, "Пользователь уже не принадлежит к данной группе");
            }

            var userInGroupRelationRepo = GetRepository<EccUserInUserGroupRelation>();

            userInGroupRelationRepo.DeleteHandled(elem);

            return await TrySaveChangesAndReturnResultAsync("Пользователь удален из группы");
        }

        public async Task<BaseApiResponse> ChangeUsersInGroupAsync(ChangeUsersInUserGroupModel model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }
            
            if (model.UserActions == null || model.UserActions.Count == 0)
            {
                return new BaseApiResponse(false, "Вы подали пустой массив изменений");
            }

            var userGroupRepo = GetRepository<EccUserGroup>();

            var userInUserGrouopRelationRepo = GetRepository<EccUserInUserGroupRelation>();

            var userGroup = await userGroupRepo.Query().Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == model.GroupId);

            if (userGroup == null)
            {
                return new BaseApiResponse(false, "Группа не найдена по указанному идентификатору");
            }

            foreach (var userToDelete in model.UserActions.Where(x => !x.AddOrDelete))
            {
                var user = userGroup.Users.FirstOrDefault(x => x.UserId == userToDelete.UserId);

                if (user != null)
                {
                    userInUserGrouopRelationRepo.DeleteHandled(user);
                }
            }

            foreach (var userToAddModel in model.UserActions.Where(x => x.AddOrDelete))
            {
                var userRelation = new EccUserInUserGroupRelation
                {
                    UserGroupId = model.GroupId,
                    UserId = userToAddModel.UserId,
                };

                userInUserGrouopRelationRepo.CreateHandled(userRelation);
            }

            return await TrySaveChangesAndReturnResultAsync("Пользователи в группе изменены");
        }

        public async Task<BaseApiResponse> RemoveUserGroupAsync(string id)
        {
            if (!IsUserAdmin())
            {
                return new BaseApiResponse(false, "У вас недостаточно прав для удаления группы");
            }

            var repo = GetRepository<EccUserGroup>();

            var elem = await repo.Query().Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);

            if (elem == null)
            {
                return new BaseApiResponse(false, "Группа не существует");
            }

            elem.Deleted = true;

            repo.UpdateHandled(elem);

            return await TrySaveChangesAndReturnResultAsync("Группа удалена");
        }

        public async Task<BaseApiResponse> AddUserToGroupAsync(UserInUserGroupIdModel model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var repo = GetRepository<EccUserGroup>();

            var group = await repo.Query().Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == model.UserGroupId);

            if (group == null)
            {
                return new BaseApiResponse(false, "Группа пользователей не найдена по указанному идентификатору");
            }

            if (!await Query<EccUser>().AnyAsync(x => x.Id == model.UserId))
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            if (group.Users.Any(x => x.UserGroupId == model.UserGroupId && x.UserId == model.UserId))
            {
                return new BaseApiResponse(false, "Пользователь уже принадлежит данной группе");
            }

            var relationRepo = GetRepository<EccUserInUserGroupRelation>();

            var userInUserGroupRelation = new EccUserInUserGroupRelation
            {
                UserId = model.UserId,
                UserGroupId = model.UserGroupId,
            };

            relationRepo.CreateHandled(userInUserGroupRelation);

            return await TrySaveChangesAndReturnResultAsync("Пользователь добавлен к группе");
        }


        public Task<List<UserGroupModelNoUsers>> GetUserGroupsAsync()
        {
            return Query<EccUserGroup>()
                .Select(UserGroupModelNoUsers.SelectExpression).ToListAsync();
        }

        public Task<UserGroupModelWithUsers> GetUserGroupWithUsers(string id)
        {
            return Query<EccUserGroup>()
                .Select(UserGroupModelWithUsers.SelectExpression).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
