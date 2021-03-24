using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Clt.Logic.Abstractions;
using Clt.Contract.Models.Common;
using Clt.Logic.Core.Workers;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Clt.Contract.Enumerations;
using Clt.Model.Entities.Default;
using Clt.Model.Entities;
using Croco.Core.Contract.Application;
using Clt.Contract.Settings;

namespace Clt.Logic.Workers.Account
{
    public class AccountManager : BaseCltWorker
    {
        RoleManager<ApplicationRole> RoleManager { get; }
        UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// Создается пользователь Root в системе и ему присваиваются все необходимые права
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponse> InitAsync()
        {
            await RoleFromEnumCreator.CreateRolesAsync<UserRight, ApplicationRole>(RoleManager);

            var maybeRoot = await UserManager.FindByEmailAsync(RightsSettings.RootEmail);

            if (maybeRoot == null)
            {
                maybeRoot = new ApplicationUser
                {
                    Email = RightsSettings.RootEmail,
                    EmailConfirmed = true,
                    UserName = RightsSettings.RootEmail,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                await UserManager.CreateAsync(maybeRoot, RightsSettings.RootPassword);

                CreateHandled(new Client
                {
                    Id = maybeRoot.Id,
                    Email = RightsSettings.RootEmail,
                    Name = RightsSettings.RootEmail
                });

                await SaveChangesAsync();
            }

            foreach (UserRight right in Enum.GetValues(typeof(UserRight)))
            {
                await UserManager.AddToRoleAsync(maybeRoot, right.ToString());
            }

            return new BaseApiResponse(true, "Пользователь root создан");
        }

        public BaseApiResponse<ApplicationUserBaseModel> CheckUserChanges(IApplicationAuthenticationManager authenticationManager, SignInManager<ApplicationUser> signInManager)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse<ApplicationUserBaseModel>(true, "Вы не авторизованы в системе", null);
            }

            return new BaseApiResponse<ApplicationUserBaseModel>(true, "", null);
        }


        public AccountManager(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication app,
            RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager) : base(ambientContext, app)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }
    }
}