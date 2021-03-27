using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Account
{
    /// <summary>
    /// Клиент забыл пароль и хочет востановить его по номеру тел
    /// </summary>
    public class ForgotPasswordModelByPhone
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать номер телефона")]
        public string PhoneNumber { get; set; }
    }
}