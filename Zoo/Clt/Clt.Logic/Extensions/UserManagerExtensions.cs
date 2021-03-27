using Clt.Contract.Enumerations;
using Clt.Model.Entities.Default;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Clt.Logic.Extensions
{
    internal static class UserManagerExtensions
    {
        /// <summary>
        /// Добавляет право пользователю
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="user"></param>
        /// <param name="userRight"></param>
        /// <returns></returns>
        public static Task<IdentityResult> AddRight(this UserManager<ApplicationUser> userManager, ApplicationUser user, UserRight userRight)
        {
            return userManager.AddToRoleAsync(user, userRight.ToString());
        }
    }
}