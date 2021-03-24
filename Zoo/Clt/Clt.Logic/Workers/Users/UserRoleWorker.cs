using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Clt.Contract.Models.Roles;
using System;
using Croco.Core.Contract;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Workers;
using Croco.Core.Logic.Extensions;
using Clt.Contract.Enumerations;
using Clt.Model.Entities.Default;
using Clt.Logic.Core.Resources;
using Croco.Core.Contract.Application;

namespace Clt.Logic.Workers.Users
{
    public class UserRoleWorker : BaseCltWorker
    {
        public static int GetTheHighestRoleOfUser(IList<string> roles)
        {
            var rights = new[]
            {
                UserRight.Root, UserRight.SuperAdmin, UserRight.Admin
            };

            foreach(var right in rights)
            {
                if(roles.Contains(right.ToString()))
                {
                    return (int)right;
                }
            }

            return int.MaxValue;
        }

        public async Task<List<ApplicationRoleModel>> GetApplicationRoles<TEnum>() where TEnum : Enum
        {
            var res = await Query<ApplicationRole>().Select(x => new ApplicationRoleModel
            {
                Id = x.Id,
                RoleName = x.Name
            }).ToListAsync();

            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

            res.ForEach(x =>
            {
                var hasEnumValue = enumValues.Any(t => t.ToString() == x.RoleName);

                if(hasEnumValue)
                {
                    x.DisplayRoleName = enumValues.FirstOrDefault(t => t.ToString() == x.RoleName).ToDisplayName();
                }
            });

            return res;
        }

        public Task<BaseApiResponse> AddUserToRoleAsync(UserIdAndRole userIdAndRole, UserManager<ApplicationUser> userManager)
        {
            return AddOrRemoveUserRoleAsync(userIdAndRole, true, userManager);
        }

        public Task<BaseApiResponse> RemoveRoleFromUserAsync(UserIdAndRole userIdAndRole, UserManager<ApplicationUser> userManager)
        {
            return AddOrRemoveUserRoleAsync(userIdAndRole, false, userManager);
        }

        public async Task<BaseApiResponse> AddOrRemoveUserRoleAsync(UserIdAndRole userIdAndRole, bool addOrRemove, UserManager<ApplicationUser> userManager)
        {
            if (!IsUserAdmin())
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAnAdministrator);
            }

            var role = await Query<ApplicationRole>().FirstOrDefaultAsync(x => x.Name == userIdAndRole.Role.ToString());

            if (role == null)
            {
                return new BaseApiResponse(false, "Право не найдено (Возможно оно еще не создано)");
            }

            var userRepo = GetRepository<ApplicationUser>();

            //Находим того, кого будем редактировать
            var user = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == userIdAndRole.UserId);

            if (user == null)
            {
                return new BaseApiResponse(false, "Изменяемый пользователь не найден");
            }

            //Находим себя 
            var userEditor = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == UserId);

            if (userEditor == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            //Находим роли редактируемого и редактора
            var rolesOfEditUser = await userManager.GetRolesAsync(user);
            var rolesOfUserEditor = await userManager.GetRolesAsync(userEditor);

            var roleOfEditUser = GetTheHighestRoleOfUser(rolesOfEditUser);
            var roleOfUserEditor = GetTheHighestRoleOfUser(rolesOfUserEditor);

            if (roleOfUserEditor >= roleOfEditUser)
            {
                return new BaseApiResponse(false, "Вы не имеете прав изменять роль данного пользователя");
            }

            if (roleOfUserEditor >= (int)userIdAndRole.Role)
            {
                return new BaseApiResponse(false, "Вы не имеете прав добавлять/удалять роль равную или выше вашей");
            }

            var userRole = await Query<ApplicationUserRole>().FirstOrDefaultAsync(x => x.RoleId == role.Id && x.UserId == user.Id);

            var isInRoleResult = userRole != null;

            if (isInRoleResult && addOrRemove)
            {
                return new BaseApiResponse(false, "У пользователя уже есть данная роль");
            }

            if (!isInRoleResult && !addOrRemove)
            {
                return new BaseApiResponse(false, "У пользователя уже и так нет данной роли");
            }

            if (addOrRemove)
            {
                CreateHandled(new ApplicationUserRole
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });
            }
            else
            {
                DeleteHandled(userRole);
            }

            return await TrySaveChangesAndReturnResultAsync(addOrRemove ? "Право добавлено" : "Право удалено");
        }

        
        public UserRoleWorker(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication app) 
            : base(ambientContext, app)
        {
        }
    }
}