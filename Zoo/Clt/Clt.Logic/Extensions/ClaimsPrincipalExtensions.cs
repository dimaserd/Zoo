using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Clt.Logic.Extensions
{
    /// <summary>
    /// Расширения для контекста авторизации
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Получить идентификатор пользователя
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Получить идентификатор пользователя
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetUserId(this IPrincipal principal)
        {
            var claim = principal as ClaimsPrincipal;

            return GetUserId(claim);
        }

        /// <summary>
        /// Получить идентификатор пользователя
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetUserId(this IIdentity principal)
        {
            var claim = new ClaimsPrincipal(principal);

            return GetUserId(claim);
        }
    }
}