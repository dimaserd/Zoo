using Ecc.Common.Enumerations;
using System;

namespace Ecc.Contract.Models.Sms
{
    /// <summary>
    /// Модель описывающя смс
    /// </summary>
    public class SmsMessageModel
    {
        /// <summary>
        /// Идентификатор
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
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Статус взаимодействия
        /// </summary>
        public InteractionStatus Status { get; set; }
    }
}