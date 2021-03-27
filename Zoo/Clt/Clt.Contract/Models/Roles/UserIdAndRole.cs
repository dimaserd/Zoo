using Clt.Contract.Enumerations;

namespace Clt.Contract.Models.Roles
{
    /// <summary>
    /// Идентифкатор пользователя с ролью
    /// </summary>
    public class UserIdAndRole
    {
        /// <summary>
        /// Идентифкатор пользователя
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Роль
        /// </summary>
        public UserRight Role { get; set; }
    }
}