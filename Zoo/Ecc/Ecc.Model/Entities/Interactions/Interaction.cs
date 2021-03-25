using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Model.Models;
using Ecc.Model.Entities.External;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Ecc.Model.Consts;
using Croco.Core.Contract.Data.Entities.HaveId;

namespace Ecc.Model.Entities.Interactions
{
    [Table(nameof(Interaction), Schema = Schemas.EccSchema)]
    public class Interaction : AuditableEntityBase, IHaveStringId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [MaxLength(24)]
        public string Type { get; set; }

        public string MessageText { get; set; }

        public string TitleText { get; set; }

        public string MaskItemsJson { get; set; }

        /// <summary>
        /// Отправить немедленно
        /// </summary>
        public bool SendNow { get; set; }

        /// <summary>
        /// Отправить в определенное время
        /// </summary>
        public DateTime? SendOn { get; set; }

        /// <summary>
        /// Сообщение было отправлено в данную дату
        /// </summary>
        public DateTime? SentOn { get; set; }

        /// <summary>
        /// Сообщение было доставлено в данную дату
        /// </summary>
        public DateTime DeliveredOn { get; set; }

        /// <summary>
        /// Сообщение было прочитано в данную дату
        /// </summary>
        public DateTime? ReadOn { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому нужно отправить сообщение
        /// </summary>
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual EccUser User { get; set; }

        [MaxLength(128)]
        public string MessageDistributionId { get; set; }

        public virtual ICollection<InteractionStatusLog> Statuses { get; set; }

        public virtual ICollection<InteractionAttachment> Attachments { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<Interaction>();

            entityBuilder
                .Property(x => x.Id)
                .ValueGeneratedNever();

            entityBuilder.HasIndex(x => x.MessageDistributionId).IsUnique(false);

            entityBuilder
                .HasIndex(x => new { x.Type })
                .IsUnique(false);

            entityBuilder
                .HasDiscriminator<string>(nameof(Type))
                .HasValue<UserNotificationInteraction>(EccConsts.InAppNotificationType)
                .HasValue<MailMessageInteraction>(EccConsts.EmailType)
                .HasValue<SmsMessageInteraction>(EccConsts.SmsType);
        }
    }
}