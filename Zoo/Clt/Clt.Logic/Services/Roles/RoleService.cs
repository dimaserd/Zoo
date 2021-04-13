using Clt.Model.Entities.Default;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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


    }
}