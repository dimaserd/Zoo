namespace Clt.Contract.Models.Users
{
    public class ClientModel : EditClient
    {
        public string Email { get; set; }

        public int? AvatarFileId { get; set; }
    }
}