namespace Ecc.Contract.Models
{
    /// <summary>
    /// Модель описывающая пользвателя в контексте рассылок
    /// </summary>
    public class EccUserModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}