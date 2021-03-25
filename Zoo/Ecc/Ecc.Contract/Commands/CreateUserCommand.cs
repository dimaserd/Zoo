namespace Ecc.Contract.Commands
{
    /// <summary>
    /// Команда для создания пользователя
    /// </summary>
    public class CreateUserCommand
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}