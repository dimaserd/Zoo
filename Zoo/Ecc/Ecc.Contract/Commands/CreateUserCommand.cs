using Ecc.Contract.Models.Users;

namespace Ecc.Contract.Commands
{
    /// <summary>
    /// Команда для создания пользователя
    /// </summary>
    public class CreateUserCommand
    {
        /// <summary>
        /// Данные пользователя
        /// </summary>
        public EccUserModel UserData { get; set; }
    }
}