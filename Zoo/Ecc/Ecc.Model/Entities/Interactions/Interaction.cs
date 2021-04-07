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
    /// <summary>
    /// Сущность описывающая взаимодействие
    /// </summary>
    [Table(nameof(Interaction), Schema = Schemas.EccSchema)]
    public class Interaction : AuditableEntityBase, IHaveStringId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        /// <summary>
        /// Тип взаимодействия
        /// </summary>
        [MaxLength(24)]
        public string Type { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Текст заголовка
        /// </summary>
        public string TitleText { get; set; }

        /// <summary>
        /// Сериализованные маски
        /// </summary>
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

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual EccUser User { get; set; }

        /// <summary>
        /// Идентификатор рассылки
        /// </summary>
        [MaxLength(128)]
        public string MessageDistributionId { get; set; }

        /// <summary>
        /// Логи статусов
        /// </summary>
        public virtual ICollection<InteractionStatusLog> Statuses { get; set; }

        /// <summary>
        /// Вложения
        /// </summary>
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