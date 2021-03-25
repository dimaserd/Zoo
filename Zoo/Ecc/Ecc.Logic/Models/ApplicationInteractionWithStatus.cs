using Ecc.Model.Entities.Interactions;
using Ecc.Common.Enumerations;

namespace Ecc.Logic.Models
{
    public class ApplicationInteractionWithStatus<TInteraction>
        where TInteraction : Interaction, new()
    {
        public TInteraction Interaction { get; set; }

        public InteractionStatus Status { get; set; }
    }
}