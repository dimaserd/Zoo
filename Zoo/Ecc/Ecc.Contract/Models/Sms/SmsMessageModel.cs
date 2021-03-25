using Ecc.Common.Enumerations;
using System;

namespace Ecc.Contract.Models.Sms
{
    public class SmsMessageModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string Header { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime? SentOn { get; set; }

        public string PhoneNumber { get; set; }

        public InteractionStatus Status { get; set; }
    }
}