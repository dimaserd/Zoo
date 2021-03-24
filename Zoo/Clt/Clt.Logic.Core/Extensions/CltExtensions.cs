using Clt.Contract.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace Clt.Logic.Core
{
    public static class CltExtensions
    {
        /// <summary>
        /// Из модели DTO в сущность
        /// </summary>
        /// <returns></returns>
        public static TUser ToEntity<TUser>(this ApplicationUserBaseModel model) where TUser : IdentityUser, new()
        {
            return new TUser
            {
                Id = model.Id,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                EmailConfirmed = model.EmailConfirmed,
                SecurityStamp = model.SecurityStamp
            };
        }
    }
}