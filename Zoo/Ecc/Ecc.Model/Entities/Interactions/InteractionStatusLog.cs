using Ecc.Common.Enumerations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Interactions
{
    /// <summary>
    /// Лог об изменении статсуса взаимодействия
    /// </summary>
    [Table(nameof(InteractionStatusLog), Schema = Schemas.EccSchema)]
    public class InteractionStatusLog
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор взаимодействия
        /// </summary>
        public string InteractionId { get; set; }

        /// <summary>
        /// Взаимодействие
        /// </summary>
        [ForeignKey(nameof(InteractionId))]
        public virtual Interaction Interaction { get; set; }

        /// <summary>
        /// Статус взаимодействия
        /// </summary>
        public InteractionStatus Status { get; set; }

        /// <summary>
        /// Дата установки статуса
        /// </summary>
        public DateTime StartedOn { get; set; }

        /// <summary>
        /// Описание статуса
        /// </summary>
        public string StatusDescription { get; set; }
    }
}