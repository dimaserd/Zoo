namespace Clt.Contract.Models.Users
{
    /// <summary>
    /// Модель описывающая зарегистрированного пользователя
    /// </summary>
    public class RegisteredUser
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }
    }
}