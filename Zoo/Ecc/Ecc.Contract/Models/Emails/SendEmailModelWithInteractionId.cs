using Ecc.Contract.Abstractions;
using Ecc.Contract.Models.Emails;

namespace Ecc.Contract.Models
{
    /// <summary>
    /// Отправить Email как взаимодейтсвие
    /// </summary>
    public class SendEmailModelWithInteractionId : ISendEmailModel
    {
        /// <summary>
        /// Идентифкатор взаимодействия
        /// </summary>
        public string InteractionId { get; set; }

        /// <summary>
        /// Модель для отправки сообщения
        /// </summary>
        public SendEmailModel EmailModel { get; set; }

        /// <summary>
        /// Предоставить модель сообщения
        /// </summary>
        /// <returns></returns>
        public SendEmailModel ToSendEmailModel()
        {
            return EmailModel;
        }
    }
}