using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.EmailGroup
{
    public class AddEmailsToEmailGroup
    {
        public string EmailGroupId { get; set; }

        [Required(ErrorMessage = "Нужно указать Email адреса")]
        public List<string> Emails { get; set; }
    }
}