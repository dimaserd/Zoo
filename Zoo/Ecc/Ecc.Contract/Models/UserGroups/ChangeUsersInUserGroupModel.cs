using System.Collections.Generic;

namespace Ecc.Contract.Models.UserGroups
{
    /// <summary>
    /// Модель добавления или удаления пользователей из группы
    /// </summary>
    public class ChangeUsersInUserGroupModel
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Действия с пользователями
        /// </summary>
        public List<UserInGroupAddOrDelete> UserActions { get; set; }
    }
}