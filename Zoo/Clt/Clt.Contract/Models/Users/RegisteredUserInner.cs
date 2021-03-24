namespace Clt.Contract.Models.Users
{
    public class RegisteredUserInner : RegisteredUser
    {
        public bool EmailConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PasswordHash { get; set; }
    }
}