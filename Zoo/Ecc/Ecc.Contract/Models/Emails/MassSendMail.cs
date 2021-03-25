using System.Collections.Generic;

namespace Ecc.Contract.Models.Emails
{
    public class MassSendMail
    {
        public string SubjectFormat { get; set; }

        public string BodyFormat { get; set; }

        public List<int> AttachmentFileIds { get; set; }

        public List<SendMailWithMaskItems> EmailWithMasks { get; set; }      
    }
}