namespace Ecc.Contract.Models.MailDistributions
{
    /// <summary>
    /// Модель описывающая привязку группы к рассылке
    /// </summary>
    public class MainDistributionUserGroupRelationIdModel
    {
        /// <summary>
        /// Идентификатор рассылки
        /// </summary>
        public string MailDistributionId { get; set; }

        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public string GroupId { get; set; }
    }
}