using Ecc.Contract.Models.Emails;

namespace Ecc.Contract.Abstractions
{
    /// <summary>
    /// Абстракция конвертируемости в модель Email
    /// </summary>
    public interface ISendEmailModel
    {
        /// <summary>
        /// Привести к модели отправки Email сообщения
        /// </summary>
        /// <returns></returns>
        SendEmailModel ToSendEmailModel();
    }
}