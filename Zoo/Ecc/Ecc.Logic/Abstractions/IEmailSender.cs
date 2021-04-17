using System.Threading.Tasks;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models.Emails;

namespace Ecc.Logic.Abstractions
{
    /// <summary>
    /// Абстракция описывающая отправителя электронной почты
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Отправить электронную почту
        /// </summary>
        /// <param name="emailModel"></param>
        /// <returns></returns>
        Task<BaseApiResponse> SendEmail(SendEmailModelWithLoadedAttachments emailModel);
    }
}