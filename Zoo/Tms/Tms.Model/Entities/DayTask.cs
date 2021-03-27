using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Model.Models;
using Newtonsoft.Json;

namespace Tms.Model.Entities
{
    /// <summary>
    /// Сущность описывающая задание на день
    /// </summary>
    public class DayTask : AuditableEntityBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DayTask()
        {
            Comments = new List<DayTaskComment>();
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Дата задания больше день чем просто дата
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime TaskDate { get; set; }

        /// <summary>
        /// Текст задания
        /// </summary>
        public string TaskText { get; set; }

        /// <summary>
        /// Название задания
        /// </summary>
        public string TaskTitle { get; set; }

        /// <summary>
        /// Дата завершения
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// Цель Html (Summernote)
        /// </summary>
        public string TaskTarget { get; set; }

        /// <summary>
        /// Отчет Html (Summernote)
        /// </summary>
        public string TaskReview { get; set; }

        /// <summary>
        /// Комментарий Html (Summernote)
        /// </summary>
        public string TaskComment { get; set; }

        /// <summary>
        /// Оценка исполнителя
        /// </summary>
        public int EstimationSeconds { get; set; }

        /// <summary>
        /// Фактически затраченное время
        /// </summary>
        public int CompletionSeconds { get; set; }

        #region Свойства отношений

        /// <summary>
        /// Идентификатор автора данного задания
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Идентифкатор пользователя на которого назначили данное задание
        /// </summary>
        public string AssigneeUserId { get; set; }

        /// <summary>
        /// Комментарии к данному заданию
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<DayTaskComment> Comments { get; set; }
        #endregion
    }
}
