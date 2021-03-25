using System.ComponentModel.DataAnnotations;
using Ecc.Common.Enumerations;

namespace Ecc.Logic.Models.Notifications
{
    public class CreateNotification
    {
        [Required(ErrorMessage = "Необходимо указать заголовок уведомления")]
        [Display(Name = "Заголовок уведомления")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо указать текст уведомления")]
        [Display(Name = "Текст уведомления")]
        public string Text { get; set; }

        public string ObjectJSON { get; set; }

        [Display(Name = "Тип уведомления")]
        public UserNotificationType Type { get; set; }

        [Required(ErrorMessage = "Необходимо указать идентификатор пользователя")]
        public string UserId { get; set; }
    }
}