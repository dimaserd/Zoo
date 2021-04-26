using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.LinkCatch
{
    /// <summary>
    /// Cущность описывающая пойманную передаресацию в электронном письме
    /// </summary>
    [Table(nameof(EmailLinkCatchRedirect), Schema = Schemas.EccSchema)]
    public class EmailLinkCatchRedirect
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентифкатор пойманной записи
        /// </summary>
        public string EmailLinkCatchId { get; set; }

        /// <summary>
        /// Дата редиректа
        /// </summary>
        public DateTime RedirectedOnUtc { get; set; }

        /// <summary>
        /// Ссылка на ловитель ссылок
        /// </summary>
        [ForeignKey(nameof(EmailLinkCatchId))]
        public EmailLinkCatch LinkCatch { get; set; }
    }
}