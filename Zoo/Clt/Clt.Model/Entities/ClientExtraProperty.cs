using Croco.Core.Model.Models;

namespace Clt.Model.Entities
{
    /// <summary>
    /// Дополнительное свойство для клиента
    /// </summary>
    public class ClientExtraProperty : AuditableEntityBase
    {
        /// <summary>
        /// Название свойства
        /// </summary>
        public string PropertyName { get; set; }
        
        /// <summary>
        /// Идентификатор клиентв
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public virtual Client Client { get; set; }
        
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