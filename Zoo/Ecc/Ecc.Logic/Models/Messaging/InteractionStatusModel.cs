using Ecc.Common.Enumerations;
using System;

namespace Ecc.Logic.Models.Messaging
{
    /// <summary>
    /// Модель описывающая статус взаимодействия
    /// </summary>
    public class InteractionStatusModel
    {
        /// <summary>
        /// Статус
        /// </summary>
        public InteractionStatus Status { get; set; }

        /// <summary>
        /// Дата назначения статуса
        /// </summary>
        public DateTime StartedOn { get; set; }

        /// <summary>
        /// Описание статуса
        /// </summary>
        public string StatusDescription { get; set; }
    }
}