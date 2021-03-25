using Ecc.Common.Enumerations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Interactions
{
    [Table(nameof(InteractionStatusLog), Schema = Schemas.EccSchema)]
    public class InteractionStatusLog
    {
        public string Id { get; set; }

        public string InteractionId { get; set; }

        [ForeignKey(nameof(InteractionId))]
        public virtual Interaction Interaction { get; set; }

        public InteractionStatus Status { get; set; }

        public DateTime StartedOn { get; set; }

        public string StatusDescription { get; set; }
    }
}