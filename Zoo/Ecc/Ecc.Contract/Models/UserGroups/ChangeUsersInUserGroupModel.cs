using System.Collections.Generic;

namespace Ecc.Contract.Models.UserGroups
{
    /// <summary>
    /// Модель добавления или удаления пользователей из группы
    /// </summary>
    public class ChangeUsersInUserGroupModel
    {
        public string GroupId { get; set; }

        public List<UserInGroupAddOrDelete> UserActions { get; set; }
    }
}