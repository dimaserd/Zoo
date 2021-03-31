namespace Ecc.Contract.Models
{
    /// <summary>
    /// Идентификатор с эмейлом
    /// </summary>
    public class UserIdAndEmailModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }
    }
}