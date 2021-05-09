namespace Clt.Contract.Models.Clients.Properties
{
    /// <summary>
    /// Добавить или обновить свойство клиента
    /// </summary>
    /// <typeparam name="TPropValue"></typeparam>
    public class AddOrUpdateClientProperty<TPropValue>
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Название свойства
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Значение свойства
        /// </summary>
        public TPropValue PropertyValue { get; set; }
    }
}