using System.Collections.Generic;

namespace Ecc.Contract.Models.Emails
{
    /// <summary>
    /// Отправить на данный Email c таким процессингом
    /// </summary>
    public class SendMailWithMaskItems
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Маски
        /// </summary>
        public List<KeyValuePair<string, string>> MaskItems { get; set; }
    }
}