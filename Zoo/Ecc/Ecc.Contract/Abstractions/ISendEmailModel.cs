using Ecc.Contract.Models.Emails;

namespace Ecc.Contract.Abstractions
{
    public interface ISendEmailModel
    {
        SendEmailModel ToSendEmailModel();
    }
}
