using System.Collections.Generic;

namespace Ecc.Logic.Models.Messaging
{
    /// <summary>
    /// Модель добавления или удаления группы пользователей из рассылки
    /// </summary>
    public class ChangeUserGroupsInMailDistributionModel
    {
        public string MailDistributionId { get; set; }

        public List<UserGroupIMailDistributionAddOrDelete> UserGroupActions { get; set; }
    }
}