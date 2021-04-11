using Clt.Model.Entities.Default;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Clt.Logic.Services.Roles
{
    public class RoleService : BaseCltService
    {
        RoleManager<ApplicationRole> RoleManager { get; }

        public RoleService(
            RoleManager<ApplicationRole> roleManager,

            ICrocoAmbientContextAccessor context, 
            ICrocoApplication application, ILogger<RoleService> logger) : base(context, application, logger)
        {
            RoleManager = roleManager;
        }


    }
}