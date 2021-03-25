using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Logic.Workers;
using Tms.Model;

namespace Tms.Logic.Services
{
    public class TmsBaseService : BaseCrocoWorker<TmsDbContext>
    {
        PrincipalCheker PrincipalCheker { get; }

        public TmsBaseService(ICrocoAmbientContextAccessor contextAccessor, 
            ICrocoApplication application,
            PrincipalCheker principalCheker) : base(contextAccessor, application)
        {
            PrincipalCheker = principalCheker;
        }

        public bool IsUserAdmin()
        {
            return PrincipalCheker.IsAdmin(User);
        }
    }
}