using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Croco.Core.Logic.Models.Users;
using Ecc.Model.Entities.Interactions;
using Newtonsoft.Json;

namespace Ecc.Logic.Models.Users
{
    public class UserMailMessageModel
    {
        public string Id { get; set; }

        public string ReceiverEmail { get; set; }

        [Display(Name = "Тело сообщения")]
        public string Body { get; set; }

        [Display(Name = "Тема письма")]
        public string Subject { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? SentDate { get; set; }

        public DateTime? ReadDate { get; set; }

        public UserIdNameEmailAvatarModel User { get; set; }

        [JsonIgnore]
        public static Expression<Func<MailMessageInteraction, UserMailMessageModel>> SelectExpression = x => new UserMailMessageModel
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