using Ecc.Common.Enumerations;
using System;

namespace Ecc.Logic.Models.Messaging
{
    /// <summary>
    /// Модель описывающая эмейл
    /// </summary>
    public class MailMessageModel
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Дата прочтения
        /// </summary>
        public DateTime? ReadOn { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public DateTime? SentOn { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Статус уведомления
        /// </summary>
        public InteractionStatus Status { get; set; }
    }
}