using Croco.Core.Contract;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Workers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Clt.Logic.Core.Workers
{
    public class PasswordHashValidator<TUser, TContext> : LightCrocoWorker<TContext> 
        where TUser : IdentityUser
        where TContext : DbContext
    {
        public PasswordHashValidator(ICrocoAmbientContextAccessor context) : base(context)
        {
        }

        public async Task<BaseApiResponse> CheckUserNameAndPasswordAsync(string userId, string userName, string pass)
        {
            var user = await Query<TUser>()
                .FirstOrDefaultAsync(x => x.Id == userId);

            var passHasher = new PasswordHasher<TUser>();

            var t = passHasher.VerifyHashedPassword(user, user.PasswordHash, pass) != PasswordVerificationResult.Failed && user.UserName == userName;

            return new BaseApiResponse(t, t? "Ok" : "Not Ok");
        }
    }
}