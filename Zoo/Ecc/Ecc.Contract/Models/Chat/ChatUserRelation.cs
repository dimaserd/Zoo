namespace Ecc.Contract.Models.Chat
{
    public class ChatUserRelation
    {
        public string UserId { get; set; }

        public int ChatId { get; set; }

        public long LastVisitUtcTicks { get; set; }
    }
}