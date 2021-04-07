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
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
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

        /// <summary>
        /// Отправлять каждому пользователю
        /// </summary>
        [Display(Name = "Отправлять каждому пользователю")]
        public bool SendToEveryUser { get; set; }

        /// <summary>
        /// Группы пользователей
        /// </summary>
        public List<UserGroupModelWithUsers> UserGroups { get; set; }
    }
}