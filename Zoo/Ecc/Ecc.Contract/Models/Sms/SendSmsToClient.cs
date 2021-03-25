using System.Collections.Generic;

namespace Ecc.Contract.Models.Sms
{
    public class SendSmsToClient
    {
        public string ClientId { get; set; }

        public string Message { get; set; }

        public List<int> AttachmentFileIds { get; set; }
    }
}