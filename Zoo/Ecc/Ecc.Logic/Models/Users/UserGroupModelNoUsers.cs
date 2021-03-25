using Ecc.Model.Entities.External;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;

namespace Ecc.Logic.Models.Users
{
    public class UserGroupModelNoUsers
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }

        [JsonIgnore]
        public static Expression<Func<EccUserGroup, UserGroupModelNoUsers>> SelectExpression = x => new UserGroupModelNoUsers
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Deleted = x.Deleted
        };
    }
}