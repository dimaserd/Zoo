namespace Ecc.Contract.Models.Chat
{
    public class UserInChatModel
    {
        public UserIdAndEmailModel User { get; set; }

        public long LastVisitUtcTicks { get; set; }
    }
}