using System.Collections.Generic;
using Croco.Core.Contract;

namespace Ecc.Contract.Models.Emails
{
    public class GetEmailSenderOptions
    {
        public ICrocoAmbientContextAccessor AmbientContextAccessor { get; set; }
        public List<EccFileData> Attachments { get; set; }
    }
}