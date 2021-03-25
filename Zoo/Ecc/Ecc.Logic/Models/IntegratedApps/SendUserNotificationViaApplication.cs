namespace Ecc.Logic.Models.IntegratedApps
{
    /// <summary>
    /// Отправить уведомление пользователю через интегрированное приложение
    /// </summary>
    public class SendUserNotificationViaApplication
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}