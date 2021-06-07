using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Clt.Model.Entities.Default;
using Croco.Core.Contract.Application;
using Clt.Contract.Settings;
using Clt.Logic.Services.Roles;

namespace Clt.Logic.Services.Account
{
    /// <summary>
    /// Методы для работы с учетными записями
    /// </summary>
    public class AccountInitiator : BaseCltService
    {
        RoleService RoleService { get; }
        UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// Создается пользователь Root в системе и ему присваиваются все необходимые права
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponse> CreateRootUserAsync()
        {
            await RoleService.CreateRolesAsync(RolesSetting.GetAllRoleNames());

            var maybeRoot = await UserManager.FindByEmailAsync(RootSettings.RootEmail);

            if (maybeRoot == null)
            {
                maybeRoot = new ApplicationUser
                {
                    Email = RootSettings.RootEmail,
                    EmailConfirmed = true,
                    UserName = RootSettings.RootEmail,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                await UserManager.CreateAsync(maybeRoot, RootSettings.RootPassword);

                await SaveChangesAsync();
            }

            await UserManager.AddToRolesAsync(maybeRoot, RolesSetting.GetAllRoleNames());

            return new BaseApiResponse(true, "Пользователь root создан");
        }

        /// <summary>
        /// Удалить пользователя root
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponse> DeleteRootUserAsync()
        {
            var root = await UserManager.FindByEmailAsync(RootSettings.RootEmail);

            if(root == null)
            {
                return new BaseApiResponse(false, "Пользователь root уже удален");
            }

            await UserManager.DeleteAsync(root);
            return new BaseApiResponse(true, "Пользователь root удалён");
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="app"></param>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        public AccountInitiator(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication app,
            RoleService roleManager, UserManager<ApplicationUser> userManager) : base(ambientContext, app)
        {
            RoleService = roleManager;
            UserManager = userManager;
        }
    }
}