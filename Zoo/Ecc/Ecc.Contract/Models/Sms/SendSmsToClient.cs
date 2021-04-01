using System.Collections.Generic;

namespace Ecc.Contract.Models.Sms
{
    /// <summary>
    /// Отправить смс клиенту
    /// </summary>
    public class SendSmsToClient
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Идентификаторы вложений
        /// </summary>
        public List<int> AttachmentFileIds { get; set; }
    }
}