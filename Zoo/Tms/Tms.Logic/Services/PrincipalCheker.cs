using System;
using System.Security.Principal;

namespace Tms.Logic.Services
{
    public class PrincipalCheker
    {
        private readonly Func<IPrincipal, bool> _isAdminFunc;

        public PrincipalCheker(Func<IPrincipal, bool> isAdminFunc)
        {
            _isAdminFunc = isAdminFunc ?? throw new NullReferenceException(nameof(isAdminFunc));
        }

        public bool IsAdmin(IPrincipal rolePrincipal)
        {
            return _isAdminFunc(rolePrincipal);
        }
    }
}