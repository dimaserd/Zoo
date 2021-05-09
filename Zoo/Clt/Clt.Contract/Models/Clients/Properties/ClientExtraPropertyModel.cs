namespace Clt.Contract.Models.Clients.Properties
{
    /// <summary>
    /// Модель описываюящая свойство клиента
    /// </summary>
    /// <typeparam name="TPropValue"></typeparam>
    public class ClientExtraPropertyModel<TPropValue>
    {
        /// <summary>
        /// Название свойства
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Значение свойства
        /// </summary>
        public TPropValue Value { get; set; }
    }
}
