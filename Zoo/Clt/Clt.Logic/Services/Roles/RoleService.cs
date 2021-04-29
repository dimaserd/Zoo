using Clt.Model.Entities.Default;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Clt.Logic.Services.Roles
{
    /// <summary>
    /// Сервис для работы с ролями приложения
    /// </summary>
    public class RoleService : BaseCltService
    {
        RoleManager<ApplicationRole> RoleManager { get; }

        /// <summary>
        /// Конструктора
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="context"></param>
        /// <param name="application"></param>
        /// <param name="logger"></param>
        public RoleService(
            RoleManager<ApplicationRole> roleManager,

            ICrocoAmbientContextAccessor context, 
            ICrocoApplication application, ILogger<RoleService> logger) : base(context, application, logger)
        {
            RoleManager = roleManager;
        }

        /// <summary>
        /// Создать роли
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> CreateRolesAsync(string[] roles)
        {
            foreach (var role in roles)
            {
                if (!await RoleManager.RoleExistsAsync(role))
                {
                    await RoleManager.CreateAsync(new ApplicationRole { Name = role, ConcurrencyStamp = Guid.NewGuid().ToString() });
                }
            }

            return new BaseApiResponse(true, "Роли созданы");
        }
    }
}