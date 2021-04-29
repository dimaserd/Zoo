using Clt.Model.Entities.Default;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Clt.Logic.Services.Account
{
    /// <summary>
    /// Валидатор хешей паролей
    /// </summary>
    public class PasswordHashValidator : BaseCltService
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="app"></param>
        public PasswordHashValidator(ICrocoAmbientContextAccessor context, ICrocoApplication app)
            : base(context, app)
        {
        }

        /// <summary>
        /// Проверить пароль
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> CheckUserNameAndPasswordAsync(string userId, string userName, string pass)
        {
            var user = await Query<ApplicationUser>()
                .FirstOrDefaultAsync(x => x.Id == userId);

            var passHasher = new PasswordHasher<ApplicationUser>();

            var t = passHasher.VerifyHashedPassword(user, user.PasswordHash, pass) != PasswordVerificationResult.Failed && user.UserName == userName;

            return new BaseApiResponse(t, t ? "Ok" : "Not Ok");
        }
    }
}