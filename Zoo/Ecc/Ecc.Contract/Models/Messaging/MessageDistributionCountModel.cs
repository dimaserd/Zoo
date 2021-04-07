namespace Ecc.Contract.Models.Messaging
{
    /// <summary>
    /// Модель описывающая рассылку
    /// </summary>
    public class MessageDistributionCountModel
    {
        /// <summary>
        /// Идентификатор рассылки
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Тип рассылки
        /// </summary>
        public string Type { get; set; }

        public string Data { get; set; }

        public int InteractionsCount { get; set; }
    }
}