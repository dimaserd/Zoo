namespace Ecc.Contract.Commands
{
    /// <summary>
    /// Команда для обновления данных пользователя
    /// </summary>
    public class UpdateUserCommand
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}