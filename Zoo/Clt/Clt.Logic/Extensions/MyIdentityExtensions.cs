using Clt.Contract.Enumerations;
using System.Security.Principal;

namespace Clt.Logic.Extensions
{
    /// <summary>
    /// Расширения
    /// </summary>
    public static class MyIdentityExtensions
    {
        /// <summary>
        /// Проверка на наличие права
        /// </summary>
        /// <param name="rolePrincipal"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool HasRight(this IPrincipal rolePrincipal, UserRight right)
        {
            return rolePrincipal.IsInRole(right.ToString());
        }
    }
}