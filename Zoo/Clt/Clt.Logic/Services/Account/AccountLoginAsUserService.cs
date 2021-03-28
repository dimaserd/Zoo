using Clt.Contract.Models.Common;
using Clt.Model.Entities.Default;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Clt.Logic.Services.Account
{
    /// <summary>
    /// Сервис для логинирования за другого пользователя
    /// </summary>
    public class AccountLoginAsUserService : BaseCltService
    {
        SignInManager<ApplicationUser> SignInManager { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        /// <param name="signInManager"></param>
        public AccountLoginAsUserService(ICrocoAmbientContextAccessor context,
            ICrocoApplication application,
            SignInManager<ApplicationUser> signInManager) : base(context, application)
        {
            SignInManager = signInManager;
        }

        /// <summary>
        /// Залогиниться как другой пользователь
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> LoginAsUserAsync(UserIdModel model)
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

            var user = await SignInManager.UserManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по укаанному идентификатору");
            }

            await SignInManager.SignInAsync(user, true);

            return new BaseApiResponse(true, $"Вы залогинены как {user.Email}");
        }
    }
}