using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.LinkCatch
{
    /// <summary>
    /// Сущность описывающая ловителя ссылок в Email
    /// </summary>
    [Table(nameof(EmailLinkCatch), Schema = Schemas.EccSchema)]
    public class EmailLinkCatch
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Урл который ловится
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Идентификатор эмейла
        /// </summary>
        public string MailMessageId { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Редиректы
        /// </summary>
        public virtual ICollection<EmailLinkCatchRedirect> Redirects { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailLinkCatch>().Property(x => x.Id).ValueGeneratedNever();
        }
    }
}