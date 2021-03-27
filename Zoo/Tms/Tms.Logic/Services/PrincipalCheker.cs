using System;
using System.Security.Principal;

namespace Tms.Logic.Services
{
    /// <summary>
    /// Проверщик прав
    /// </summary>
    public class PrincipalCheker
    {
        private readonly Func<IPrincipal, bool> _isAdminFunc;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="isAdminFunc"></param>
        public PrincipalCheker(Func<IPrincipal, bool> isAdminFunc)
        {
            _isAdminFunc = isAdminFunc ?? throw new NullReferenceException(nameof(isAdminFunc));
        }

        /// <summary>
        /// Определение администратора
        /// </summary>
        /// <param name="rolePrincipal"></param>
        /// <returns></returns>
        public bool IsAdmin(IPrincipal rolePrincipal)
        {
            return _isAdminFunc(rolePrincipal);
        }
    }
}