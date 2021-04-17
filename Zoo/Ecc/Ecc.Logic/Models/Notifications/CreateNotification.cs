using System.ComponentModel.DataAnnotations;
using Ecc.Common.Enumerations;

namespace Ecc.Logic.Models.Notifications
{
    /// <summary>
    /// Модель для создания уведомления
    /// </summary>
    public class CreateNotification
    {
        /// <summary>
        /// Заголовок уведомления
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать заголовок уведомления")]
        [Display(Name = "Заголовок уведомления")]
        public string Title { get; set; }

        /// <summary>
        /// Текст уведомления
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать текст уведомления")]
        [Display(Name = "Текст уведомления")]
        public string Text { get; set; }

        /// <summary>
        /// Дополнительные данные
        /// </summary>
        public string ObjectJSON { get; set; }

        /// <summary>
        /// Тип уведомления
        /// </summary>
        [Display(Name = "Тип уведомления")]
        public UserNotificationType Type { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать идентификатор пользователя")]
        public string UserId { get; set; }
    }
}