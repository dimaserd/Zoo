using System.Threading.Tasks;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models.Emails;

namespace Ecc.Logic.Abstractions
{
    public interface IEmailSender
    {
        Task<BaseApiResponse> SendEmail(SendEmailModelWithLoadedAttachments emailModel);
    }
}