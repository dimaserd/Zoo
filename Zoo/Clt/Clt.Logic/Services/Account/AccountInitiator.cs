using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Clt.Contract.Models.Common;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Clt.Model.Entities.Default;
using Clt.Model.Entities;
using Croco.Core.Contract.Application;
using Clt.Contract.Settings;
using Clt.Logic.Extensions;

namespace Clt.Logic.Services.Account
{
    /// <summary>
    /// Методы для работы с учетными записями
    /// </summary>
    public class AccountInitiator : BaseCltService
    {
        RoleManager<ApplicationRole> RoleManager { get; }
        UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// Создается пользователь Root в системе и ему присваиваются все необходимые права
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponse> InitAsync()
        {
            await RoleManager.CreateRolesAsync(RolesSetting.GetAllRoleNames());

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

                CreateHandled(new Client
                {
                    Id = maybeRoot.Id,
                    Email = RootSettings.RootEmail,
                    Name = RootSettings.RootEmail
                });

                await SaveChangesAsync();
            }

            await UserManager.AddToRolesAsync(maybeRoot, RolesSetting.GetAllRoleNames());

            return new BaseApiResponse(true, "Пользователь root создан");
        }

        /// <summary>
        /// Проверить изменения пользователя
        /// </summary>
        /// <returns></returns>
        public BaseApiResponse<ApplicationUserBaseModel> CheckUserChanges()
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse<ApplicationUserBaseModel>(true, "Вы не авторизованы в системе", null);
            }

            return new BaseApiResponse<ApplicationUserBaseModel>(true, "", null);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="app"></param>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        public AccountInitiator(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication app,
            RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager) : base(ambientContext, app)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }
    }
}