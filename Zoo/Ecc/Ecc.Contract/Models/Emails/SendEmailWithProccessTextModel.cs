using System.Collections.Generic;

namespace Ecc.Contract.Models.Emails
{
    public class SendEmailWithProccessTextModel
    {
        public string SubjectFormat { get; set; }

        public string BodyFormat { get; set; }

        public string Email { get; set; }

        public int[] AttachmentFileIds { get; set; }

        public List<KeyValuePair<string, string>> MaskItems { get; set; }
    }
}