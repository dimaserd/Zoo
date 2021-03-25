using System.Collections.Generic;

namespace Ecc.Contract.Models.Emails
{
    public class SendMailWithMaskItems
    {
        public string Email { get; set; }

        public List<KeyValuePair<string, string>> MaskItems { get; set; }
    }
}