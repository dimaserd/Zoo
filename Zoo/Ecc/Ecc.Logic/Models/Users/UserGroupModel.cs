using Croco.Core.Logic.Models.Users;
using Ecc.Model.Entities.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ecc.Logic.Models.Users
{
    /// <summary>
    /// Группа пользователей
    /// </summary>
    public class UserGroupModelWithUsers
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Признак удаленности
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Пользователи принадлежащие к данной группе
        /// </summary>
        public List<UserIdNameEmailAvatarModel> Users { get; set; }

        internal static Expression<Func<EccUserGroup, UserGroupModelWithUsers>> SelectExpression = x => new UserGroupModelWithUsers
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Deleted = x.Deleted,
            Users = x.Users.Select(t => new UserIdNameEmailAvatarModel
            {
                Id = t.User.Id,
                Email = t.User.Email
            }).ToList()
        };
    }
}