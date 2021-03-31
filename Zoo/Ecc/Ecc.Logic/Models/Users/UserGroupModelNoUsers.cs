using Ecc.Model.Entities.External;
using System;
using System.Linq.Expressions;

namespace Ecc.Logic.Models.Users
{
    /// <summary>
    /// Модель описывающая группу без пользователей
    /// </summary>
    public class UserGroupModelNoUsers
    {
        /// <summary>
        /// Идентификатор
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


        internal static Expression<Func<EccUserGroup, UserGroupModelNoUsers>> SelectExpression = x => new UserGroupModelNoUsers
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Deleted = x.Deleted
        };
    }
}