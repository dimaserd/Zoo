using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Clt.Contract.Models.Roles;
using Croco.Core.Contract;
using Croco.Core.Contract.Models;
using Clt.Model.Entities.Default;
using Croco.Core.Contract.Application;
using Clt.Contract.Resources;

namespace Clt.Logic.Services.Roles
{
    /// <summary>
    /// Сервис для работы с ролями пользователей
    /// </summary>
    public class UserRoleService : BaseCltService
    {
        UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="app"></param>
        /// <param name="userManager"></param>
        public UserRoleService(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication app,
            UserManager<ApplicationUser> userManager)
            : base(ambientContext, app)
        {
            UserManager = userManager;
        }

        /// <summary>
        /// Получить список ролей пользователя
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationRoleModel>> GetApplicationRoles(Dictionary<string, string> displayRoleNamesStorage = null)
        {
            var res = await Query<ApplicationRole>().Select(x => new ApplicationRoleModel
            {
                Id = x.Id,
                RoleName = x.Name
            }).ToListAsync();

            if (displayRoleNamesStorage == null)
            {
                return res;
            }

            res.ForEach(x =>
            {
                if (displayRoleNamesStorage.TryGetValue(x.RoleName, out var displayRoleName))
                {
                    x.DisplayRoleName = displayRoleName;
                }
            });

            return res;
        }

        /// <summary>
        /// Добавить роль пользователю
        /// </summary>
        /// <param name="userIdAndRole"></param>
        /// <param name="checkUserWhoEdits"></param>
        /// <returns></returns>
        public Task<BaseApiResponse> AddUserToRoleAsync(UserIdAndRole userIdAndRole, bool checkUserWhoEdits)
        {
            return AddOrRemoveUserRoleAsync(userIdAndRole, true, checkUserWhoEdits);
        }

        /// <summary>
        /// Удалить роль у пользователя
        /// </summary>
        /// <param name="userIdAndRole"></param>
        /// <param name="checkUserWhoEdits"></param>
        /// <returns></returns>
        public Task<BaseApiResponse> RemoveRoleFromUserAsync(UserIdAndRole userIdAndRole, bool checkUserWhoEdits = true)
        {
            return AddOrRemoveUserRoleAsync(userIdAndRole, false, checkUserWhoEdits);
        }


        private async Task<BaseApiResponse> CheckUserWhoEdits(ApplicationUser user)
        {
            var userRepo = GetRepository<ApplicationUser>();

            //Находим себя 
            var userEditor = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == UserId);

            if (userEditor == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            //Находим роли редактируемого и редактора
            var rolesOfEditUser = await UserManager.GetRolesAsync(user);
            var rolesOfUserEditor = await UserManager.GetRolesAsync(userEditor);

            if ((rolesOfEditUser.Contains(RolesSetting.RootRoleName) || rolesOfUserEditor.Contains(RolesSetting.AdminRoleName)) && !rolesOfUserEditor.Contains(RolesSetting.RootRoleName))
            {
                return new BaseApiResponse(false, "Вы не имеете прав изменять роли администратора");
            }

            return new BaseApiResponse(true, "Ok");
        }

        private async Task<BaseApiResponse> AddOrRemoveUserRoleAsync(UserIdAndRole userIdAndRole, bool addOrRemove, bool checkUserWhoEdits = true)
        {
            if (!IsUserAdmin())
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAnAdministrator);
            }

            var role = await Query<ApplicationRole>().FirstOrDefaultAsync(x => x.Name == userIdAndRole.Role);

            if (role == null)
            {
                return new BaseApiResponse(false, "Роль не найдена (Возможно она еще не создана)");
            }

            var userRepo = GetRepository<ApplicationUser>();

            //Находим того, кого будем редактировать
            var user = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == userIdAndRole.UserId);

            if (user == null)
            {
                return new BaseApiResponse(false, "Изменяемый пользователь не найден");
            }

            if (checkUserWhoEdits)
            {
                var checkResponse = await CheckUserWhoEdits(user);

                if (!checkResponse.IsSucceeded)
                {
                    return checkResponse;
                }
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

            return await TrySaveChangesAndReturnResultAsync(addOrRemove ? "Роль добавлено" : "Роль удалена");
        }
    }
}