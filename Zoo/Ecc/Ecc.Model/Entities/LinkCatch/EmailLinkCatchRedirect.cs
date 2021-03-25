using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.LinkCatch
{
    [Table(nameof(EmailLinkCatchRedirect), Schema = Schemas.EccSchema)]
    public class EmailLinkCatchRedirect
    {
        public int Id { get; set; }

        public string EmailLinkCatchId { get; set; }

        public DateTime RedirectedOnUtc { get; set; }

        [ForeignKey(nameof(EmailLinkCatchId))]
        public EmailLinkCatch LinkCatch { get; set; }
    }
}