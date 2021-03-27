using Croco.Core.Contract.Files;

namespace Ecc.Contract.Models.Emails
{
    public class SendEmailModelWithLoadedAttachments
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string Email { get; set; }

        public IFileData[] AttachmentFiles { get; set; }
    }
}