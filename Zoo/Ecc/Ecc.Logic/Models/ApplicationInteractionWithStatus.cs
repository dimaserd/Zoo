using Ecc.Model.Entities.Interactions;
using Ecc.Common.Enumerations;

namespace Ecc.Logic.Models
{
    /// <summary>
    /// Модель описываюая взаимодействие с последним статусом
    /// </summary>
    /// <typeparam name="TInteraction"></typeparam>
    public class ApplicationInteractionWithStatus<TInteraction>
        where TInteraction : Interaction, new()
    {
        /// <summary>
        /// Взаимодействие
        /// </summary>
        public TInteraction Interaction { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public InteractionStatus Status { get; set; }
    }
}