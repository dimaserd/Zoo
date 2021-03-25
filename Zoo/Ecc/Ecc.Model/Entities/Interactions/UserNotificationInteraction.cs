using Ecc.Common.Enumerations;

namespace Ecc.Model.Entities.Interactions
{
    public class UserNotificationInteraction : Interaction
    {
        public string ObjectJson { get; set; }

        public UserNotificationType NotificationType { get; set; }
    }
}