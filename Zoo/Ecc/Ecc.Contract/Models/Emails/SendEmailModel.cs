namespace Ecc.Contract.Models.Emails
{
    public class SendEmailModel
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string Email { get; set; }

        public int[] AttachmentFileIds { get; set; }
    }
}