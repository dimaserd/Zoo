using Microsoft.AspNetCore.Identity;

namespace Clt.Model.Entities.Default
{
    /// <summary>
    /// Сущность описывающая пользователя имеющего определнную роль
    /// </summary>
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public virtual ApplicationRole Role { get; set; }
    }
}