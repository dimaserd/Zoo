using System.Collections.Generic;

namespace Ecc.Contract.Models.Emails
{
    /// <summary>
    /// Массовая отправка сообщений
    /// </summary>
    public class MassSendMail
    {
        /// <summary>
        /// Формат заголовка
        /// </summary>
        public string SubjectFormat { get; set; }

        /// <summary>
        /// Формат тела сообщения
        /// </summary>
        public string BodyFormat { get; set; }

        /// <summary>
        /// Файлы вложений
        /// </summary>
        public int[] AttachmentFileIds { get; set; }

        /// <summary>
        /// Маски для каждого email
        /// </summary>
        public List<SendMailWithMaskItems> EmailWithMasks { get; set; }      
    }
}