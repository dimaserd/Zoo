using Clt.Contract.Models.Common;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Clt.Logic.Core.Workers
{
    public class CoreCltAccountLoginWorker<TUser, TDbContext> : BaseCltWorker<TDbContext> 
        where TUser : IdentityUser
        where TDbContext : DbContext
    {
        public CoreCltAccountLoginWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        public async Task<BaseApiResponse> LoginAsUserAsync(UserIdModel model, SignInManager<TUser> signInManager)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (!IsUserRoot())
            {
                return new BaseApiResponse(false, "У вас недостаточно прав для логинирования за другого пользователя");
            }

            var user = await signInManager.UserManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по укаанному идентификатору");
            }

            await signInManager.SignInAsync(user, true);

            return new BaseApiResponse(true, $"Вы залогинены как {user.Email}");
        }
    }
}