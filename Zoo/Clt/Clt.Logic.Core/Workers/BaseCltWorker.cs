using Clt.Contract.Settings;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Logic.Workers;
using Microsoft.EntityFrameworkCore;

namespace Clt.Logic.Core.Workers
{
    public class BaseCltWorker<TDbContext> : BaseCrocoWorker<TDbContext>
        where TDbContext : DbContext
    {
        protected CltRolesSetting RolesSetting { get; }

        public BaseCltWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
            RolesSetting = Application.SettingsFactory.GetSetting<CltRolesSetting>();
        }

        public bool IsUserRoot()
        {
            return User.IsInRole(RolesSetting.RootRoleName);
        }
    }
}