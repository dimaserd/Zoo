using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Clt.Model.Entities.Default
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}