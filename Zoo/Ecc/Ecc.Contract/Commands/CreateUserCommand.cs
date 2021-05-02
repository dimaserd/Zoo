namespace Ecc.Contract.Commands
{
    /// <summary>
    /// Команда для создания пользователя
    /// </summary>
    public class CreateUserCommand
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Эмейл
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}