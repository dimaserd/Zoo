using Ecc.Contract.Models.EmailRedirects;
using System;
using System.Collections.Generic;

namespace Ecc.Logic.Models.Messaging
{
    /// <summary>
    /// Детальная модель для рассылки эмейлов
    /// </summary>
    public class MailMessageDetailedModel
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
        /// Заголовок сообщения
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
        /// Куда было отправлено сообщение
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// История статусов взаимодействия
        /// </summary>
        public List<InteractionStatusModel> Statuses { get; set; }

        /// <summary>
        /// Редиректы
        /// </summary>
        public List<EmailLinkCatchRedirectsCountModel> Redirects { get; set; }
    }
}