namespace Clt.Contract.Models.Clients.Properties
{
    /// <summary>
    /// Сырая модель описывающая дополнительное свойство клиента
    /// </summary>
    public class ClientExtraPropertyRawModel
    {
        /// <summary>
        /// Название свойства
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Тип данных спрятанного там объекта
        /// </summary>
        public string TypeFullName { get; set; }

        /// <summary>
        /// Сериализованные данные значения свойства
        /// </summary>
        public string ValueDataJson { get; set; }
    }
}
