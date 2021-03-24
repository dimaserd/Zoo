using Clt.Logic.Core.Resources;
using Clt.Logic.Settings;
using Clt.Model;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Workers;

namespace Clt.Logic.Workers
{
    public class BaseCltWorker : BaseCrocoWorker<CltDbContext>
    {
        public BaseCltWorker(ICrocoAmbientContextAccessor contextAccessor, ICrocoApplication application) : base(contextAccessor, application)
        {
        }

        public bool IsUserAdmin()
        {
            return User.IsInRole(RightsSettings.AdminRoleName);
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