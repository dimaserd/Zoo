namespace Ecc.Contract.Models.EmailGroup
{
    public class SendMailsForEmailGroup
    {
        public string EmailGroupId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public int[] AttachmentFileIds { get; set; }
    }
}