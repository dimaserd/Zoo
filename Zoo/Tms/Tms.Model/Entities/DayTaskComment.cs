using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Model.Models;
using Newtonsoft.Json;

namespace Tms.Model.Entities
{
    /// <summary>
    /// Комментарий к заданию от пользователя
    /// </summary>
    public class DayTaskComment : AuditableEntityBase
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Идентификатор задания к которому был оставлен комментарий
        /// </summary>
        [ForeignKey(nameof(DayTask))]
        public string DayTaskId { get; set; }

        /// <summary>
        /// Задание к которому был оставлен комментарий
        /// </summary>
        [JsonIgnore]
        public virtual DayTask DayTask { get; set; }

        /// <summary>
        /// Идентификатор автора данного комменатрия к заданию
        /// </summary>
        public string AuthorId { get; set; }
    }
}