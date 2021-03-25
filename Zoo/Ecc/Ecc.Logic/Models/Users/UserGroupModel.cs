using Croco.Core.Logic.Models.Users;
using Ecc.Model.Entities.External;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ecc.Logic.Models.Users
{
    public class UserGroupModelWithUsers
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public bool Deleted { get; set; }

        /// <summary>
        /// Пользователи принадлежащие к данной группе
        /// </summary>
        public List<UserIdNameEmailAvatarModel> Users { get; set; }

        [JsonIgnore]
        public static Expression<Func<EccUserGroup, UserGroupModelWithUsers>> SelectExpression = x => new UserGroupModelWithUsers
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