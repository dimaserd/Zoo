using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Croco.Core.Logic.Models.Users;
using Ecc.Model.Entities.Interactions;

namespace Ecc.Logic.Models.Users
{
    /// <summary>
    /// Письмо для пользователя
    /// </summary>
    public class UserMailMessageModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Адрес получателя
        /// </summary>
        public string ReceiverEmail { get; set; }

        /// <summary>
        /// Тело сообщения
        /// </summary>
        [Display(Name = "Тело сообщения")]
        public string Body { get; set; }

        /// <summary>
        /// Тема письма
        /// </summary>
        [Display(Name = "Тема письма")]
        public string Subject { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Дата прочтения
        /// </summary>
        public DateTime? ReadDate { get; set; }

        /// <summary>
        /// Получатель
        /// </summary>
        public UserIdNameEmailAvatarModel User { get; set; }

        internal static Expression<Func<MailMessageInteraction, UserMailMessageModel>> SelectExpression = x => new UserMailMessageModel
        {
            Id = x.Id,
            CreationDate = x.CreatedOn,
            Body = x.MessageText,
            ReadDate = x.ReadOn,
            ReceiverEmail = x.ReceiverEmail,
            SentDate = x.SentOn,
            Subject = x.TitleText,
            User = new UserIdNameEmailAvatarModel
            {
                Id = x.User.Id,
                Email = x.User.Email
            }
        };
    }
}