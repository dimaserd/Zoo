using Ecc.Contract.Abstractions;
using Ecc.Contract.Models.Emails;

namespace Ecc.Contract.Models
{
    public class SendEmailModelWithInteractionId : ISendEmailModel
    {
        public string InteractionId { get; set; }

        public SendEmailModel EmailModel { get; set; }

        public SendEmailModel ToSendEmailModel()
        {
            return EmailModel;
        }
    }
}