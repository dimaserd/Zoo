using Clt.Contract.Enumerations;
using System.Security.Principal;

namespace Clt.Logic.Extensions
{
    /// <summary>
    /// Расширения
    /// </summary>
    public static class MyIdentityExtensions
    {
        public static bool IsAdmin(this IPrincipal rolePrincipal)
        {
            return rolePrincipal.HasRight(UserRight.Admin) || rolePrincipal.HasRight(UserRight.SuperAdmin) || rolePrincipal.HasRight(UserRight.Root);
        }

        public static bool HasRight(this IPrincipal rolePrincipal, UserRight right)
        {
            return rolePrincipal.IsInRole(right.ToString());
        }
    }
}