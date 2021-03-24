using Clt.Contract.Settings;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Logic.Workers;
using Microsoft.EntityFrameworkCore;

namespace Clt.Logic.Core.Workers
{
    public class BaseCltCoreWorker<TDbContext> : BaseCrocoWorker<TDbContext>
        where TDbContext : DbContext
    {
        protected CltRolesSetting RolesSetting { get; }
        protected RightsSettings RightsSettings { get; }

        public BaseCltCoreWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
            var settingsFactory = Application.SettingsFactory;
            RolesSetting = settingsFactory.GetSetting<CltRolesSetting>();
            RightsSettings = settingsFactory.GetSetting<RightsSettings>();
        }

        public bool IsUserRoot()
        {
            return User.IsInRole(RolesSetting.RootRoleName);
        }
    }
}