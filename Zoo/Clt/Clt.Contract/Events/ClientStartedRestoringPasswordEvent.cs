namespace Clt.Contract.Events
{
    /// <summary>
    /// Клиент начал оперцаю сброса пароля
    /// </summary>
    public class ClientStartedRestoringPasswordEvent
    {
        /// <summary>
        /// Код для сброса
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }
    }
}