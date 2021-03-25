using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.LinkCatch
{
    [Table(nameof(EmailLinkCatch), Schema = Schemas.EccSchema)]
    public class EmailLinkCatch
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public string MailMessageId { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public virtual ICollection<EmailLinkCatchRedirect> Redirects { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailLinkCatch>().Property(x => x.Id).ValueGeneratedNever();
        }
    }
}