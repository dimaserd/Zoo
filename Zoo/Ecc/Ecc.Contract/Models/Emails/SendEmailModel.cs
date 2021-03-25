using System.Collections.Generic;

namespace Ecc.Contract.Models.Emails
{
    public class SendEmailModel
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string Email { get; set; }

        public List<int> AttachmentFileIds { get; set; }
    }
}