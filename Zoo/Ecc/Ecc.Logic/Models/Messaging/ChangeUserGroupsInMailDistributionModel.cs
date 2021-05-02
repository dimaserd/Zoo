using System.Collections.Generic;

namespace Ecc.Logic.Models.Messaging
{
    /// <summary>
    /// Модель добавления или удаления группы пользователей из рассылки
    /// </summary>
    public class ChangeUserGroupsInMailDistributionModel
    {
        /// <summary>
        /// Идентификатор рассылки
        /// </summary>
        public string MailDistributionId { get; set; }

        /// <summary>
        /// Действия с группами пользователей
        /// </summary>
        public List<UserGroupIMailDistributionAddOrDelete> UserGroupActions { get; set; }
    }
}