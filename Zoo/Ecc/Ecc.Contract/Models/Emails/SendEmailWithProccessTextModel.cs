using System.Collections.Generic;

namespace Ecc.Contract.Models.Emails
{
    /// <summary>
    /// Отправить Email c препроцессингом текста
    /// </summary>
    public class SendEmailWithProccessTextModel
    {
        /// <summary>
        /// Формат темы письма
        /// </summary>
        public string SubjectFormat { get; set; }

        /// <summary>
        /// Формат тела письма
        /// </summary>
        public string BodyFormat { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Идентификаторы файлов вложений
        /// </summary>
        public int[] AttachmentFileIds { get; set; }

        /// <summary>
        /// Маски для замены
        /// </summary>
        public List<KeyValuePair<string, string>> MaskItems { get; set; }
    }
}