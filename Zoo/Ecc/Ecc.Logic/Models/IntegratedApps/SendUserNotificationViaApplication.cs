namespace Ecc.Logic.Models.IntegratedApps
{
    /// <summary>
    /// Отправить уведомление пользователю через интегрированное приложение
    /// </summary>
    public class SendUserNotificationViaApplication
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Заголовок уведомления
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст уведомления
        /// </summary>
        public string Text { get; set; }
    }
}