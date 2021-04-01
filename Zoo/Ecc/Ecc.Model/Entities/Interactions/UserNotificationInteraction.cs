using Ecc.Common.Enumerations;

namespace Ecc.Model.Entities.Interactions
{
    /// <summary>
    /// Взаимодействие на сайте
    /// </summary>
    public class UserNotificationInteraction : Interaction
    {
        /// <summary>
        /// Дополнительные данные
        /// </summary>
        public string ObjectJson { get; set; }

        /// <summary>
        /// Тип уведомления
        /// </summary>
        public UserNotificationType NotificationType { get; set; }
    }
}