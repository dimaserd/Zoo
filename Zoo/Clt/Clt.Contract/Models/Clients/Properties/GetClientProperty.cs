namespace Clt.Contract.Models.Clients.Properties
{
    /// <summary>
    /// Получить свойство клиента
    /// </summary>
    public class GetClientProperty
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Название свойства
        /// </summary>
        public string PropertyName { get; set; }
    }
}