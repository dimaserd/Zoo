using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Workers;
using Ecc.Contract.Settings;
using Ecc.Logic.Resources;
using Ecc.Model.Contexts;

namespace Ecc.Logic.Workers.Base
{
    public class BaseEccWorker : BaseCrocoWorker<EccDbContext>
    {
        EccRolesSetting RolesSetting { get; }

        public BaseEccWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
            RolesSetting = Application.SettingsFactory.GetSetting<EccRolesSetting>();
        }

        public bool IsUserAdmin()
        {
            return User.IsInRole(RolesSetting.AdminRoleName);
        }

        public BaseApiResponse ValidateModelAndUserIsAdmin(object model)
        {
            var right = IsUserAdmin();

            if (!right)
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAnAdministrator);
            }

            return ValidateModel(model);
        }
    }
}