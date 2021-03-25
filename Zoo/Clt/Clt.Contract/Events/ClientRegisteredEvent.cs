namespace Clt.Contract.Events
{
    /// <summary>
    /// Событие описывающие регистрациию клиента
    /// </summary>
    public class ClientRegisteredEvent
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Сгененрированный пароль
        /// </summary>
        public bool IsPasswordGenerated { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}