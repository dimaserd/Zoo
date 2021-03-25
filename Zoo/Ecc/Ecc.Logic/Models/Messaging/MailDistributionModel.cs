using Croco.Core.Logic.Models.Users;
using Ecc.Logic.Models.Users;
using Ecc.Model.Entities.Ecc.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Ecc.Logic.Models.Messaging
{
    /// <summary>
    /// Рассылка пользователям по почте
    /// </summary>
    public class MailDistributionModel
    {
        public string Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        /// <summary>
        /// Заголовок сообщения
        /// </summary>
        [Display(Name = "Заголовок сообщения")]
        public string Subject { get; set; }

        /// <summary>
        /// Тело сообщения
        /// </summary>
        [Display(Name = "Тело сообщения")]
        public string Body { get; set; }

        [Display(Name = "Отправлять каждому пользователю")]
        public bool SendToEveryUser { get; set; }

        public List<UserGroupModelWithUsers> UserGroups { get; set; }

        [JsonIgnore]
        public static Expression<Func<MailDistribution, MailDistributionModel>> SelectExpression = x => new MailDistributionModel
        {
            Id = x.Id,
            Name = x.Name,
            Body = x.Body,
            Subject = x.Subject,
            SendToEveryUser = x.SendToEveryUser,
            UserGroups = x.UserGroups.Select(t => t.UserGroup).Select(z => new UserGroupModelWithUsers
            {
                Id = z.Id,
                Name = z.Name,
                Users = z.Users.Select(t => new UserIdNameEmailAvatarModel
                {
                    Id = t.User.Id,
                    Email = t.User.Email
                }).ToList()
            }).ToList()
        };
    }
}