using System;

namespace Tms.Logic.Models
{
    public class DayTaskWithCommentsModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Дата задания больше день чем просто дата
        /// </summary>
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
        /// Цель Html
        /// </summary>
        public string TaskTarget { get; set; }

        /// <summary>
        /// Отчет Html
        /// </summary>
        public string TaskReview { get; set; }

        /// <summary>
        /// Комментарий Html
        /// </summary>
        public string TaskComment { get; set; }

        /// <summary>
        /// Администратор
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public string AssigneeUserId { get; set; }
    }
}