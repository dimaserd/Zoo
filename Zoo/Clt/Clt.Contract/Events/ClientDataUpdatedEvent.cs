namespace Clt.Contract.Events
{
    /// <summary>
    /// Данные клиента обновлены
    /// </summary>
    public class ClientDataUpdatedEvent
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }
    }
}