namespace Ecc.Contract.Models.Chat
{
    /// <summary>
    /// Модель описывающая пользователя в чате
    /// </summary>
    public class UserInChatModel
    {
        ///Пользователь
        public UserIdAndEmailModel User { get; set; }

        /// <summary>
        /// Дата последнего посещения
        /// </summary>
        public long LastVisitUtcTicks { get; set; }
    }
}