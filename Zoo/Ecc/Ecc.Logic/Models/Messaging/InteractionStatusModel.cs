using Ecc.Common.Enumerations;
using System;

namespace Ecc.Logic.Models.Messaging
{
    public class InteractionStatusModel
    {
        public InteractionStatus Status { get; set; }

        public DateTime StartedOn { get; set; }

        public string StatusDescription { get; set; }
    }
}