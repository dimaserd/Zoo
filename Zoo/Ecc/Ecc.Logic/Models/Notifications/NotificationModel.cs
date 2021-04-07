using System;
using Ecc.Common.Enumerations;

namespace Ecc.Logic.Models.Notifications
{
    /// <summary>
    /// Модель описывающая уведомление
    /// </summary>
    public class NotificationModel
    {
        /// <summary>
        /// Идентификатор уведомления
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст уведомления
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Сериализованные дополнительные данные
        /// </summary>
        public string ObjectJson { get; set; }

        /// <summary>
        /// Тип уведомления
        /// </summary>
        public UserNotificationType Type { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Дата прочтения
        /// </summary>
        public DateTime? ReadOn { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }
    }
}