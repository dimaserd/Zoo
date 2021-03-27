namespace Clt.Contract.Models.Account
{
    /// <summary>
    /// Модель описывающая результат авторизации
    /// </summary>
    public class LoginResultModel
    {
        /// <summary>
        /// Результат авторизации
        /// </summary>
        public LoginResult Result { get; set; }

        /// <summary>
        /// Идентификатор токена
        /// </summary>
        public string TokenId { get; set; }
    }
}