using Clt.Contract.Enumerations;

namespace Clt.Contract.Models.Roles
{
    public class UserIdAndRole
    {
        public string UserId { get; set; }
        public UserRight Role { get; set; }
    }
}