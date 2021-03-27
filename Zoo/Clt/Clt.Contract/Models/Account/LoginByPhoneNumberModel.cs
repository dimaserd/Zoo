using Clt.Contract.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace Clt.Logic.Models.Account
{
    /// <summary>
    /// Залогиниться по номеру телефона
    /// </summary>
    public class LoginByPhoneNumberModel : LoginModelBase
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}