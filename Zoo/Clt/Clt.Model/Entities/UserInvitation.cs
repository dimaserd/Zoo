using Newtonsoft.Json;

namespace Clt.Model.Entities
{
    public class UserInvitation
    {
        public string Id { get; set; }

        public string SenderUserId { get; set; }

        [JsonIgnore]
        public virtual Client SenderUser { get; set; }

        public string ReceiverUserId { get; set; }

        [JsonIgnore]
        public virtual Client ReceiverUser { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}