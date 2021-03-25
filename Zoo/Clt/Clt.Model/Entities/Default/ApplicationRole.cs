using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Clt.Model.Entities.Default
{
    /// <summary>
    /// Роль в приложении
    /// </summary>
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// Пользователи имеющие данную роль
        /// </summary>
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}