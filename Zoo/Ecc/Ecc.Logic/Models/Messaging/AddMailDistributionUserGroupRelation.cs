namespace Ecc.Logic.Models.Messaging
{
    /// <summary>
    /// Добавить группу польщователей к рассылке
    /// </summary>
    public class AddMailDistributionUserGroupRelation
    {
        /// <summary>
        /// Идентификатор рассылки
        /// </summary>
        public string MailDistributionId { get; set; }

        /// <summary>
        /// Идентификатор группы пользователей
        /// </summary>
        public string UserGroupId { get;set;}
    }
}