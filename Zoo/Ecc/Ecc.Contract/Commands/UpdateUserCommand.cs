using Ecc.Contract.Models.Users;

namespace Ecc.Contract.Commands
{
    /// <summary>
    /// Команда для обновления данных пользователя
    /// </summary>
    public class UpdateUserCommand
    {
        /// <summary>
        /// Данные пользователя
        /// </summary>
        public EccUserModel UserData { get; set; }
    }
}