namespace Clt.Contract.Models.Account
{
    /// <summary>
    /// Изменить пароль используя токен
    /// </summary>
    public class ChangePasswordByToken
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Новый пароль
        /// </summary>
        public string NewPassword { get; set; }
    }
}