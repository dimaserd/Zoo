namespace Ecc.Contract.Commands
{
    /// <summary>
    /// Команда для обновления данных пользователя
    /// </summary>
    public class UpdateUserCommand
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Адрес электоронной почты
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}