namespace Clt.Contract.Models.Account
{
    public class ChangePasswordByToken
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}