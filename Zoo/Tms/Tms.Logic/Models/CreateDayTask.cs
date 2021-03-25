using System;

namespace Tms.Logic.Models
{
    public class CreateOrUpdateDayTask
    {
        public string Id { get; set; }

        public DateTime TaskDate { get; set; }

        /// <summary>
        /// Описание задания
        /// </summary>
        public string TaskText { get; set; }

        /// <summary>
        /// Название задания
        /// </summary>
        public string TaskTitle { get; set; }

        public string AssigneeUserId { get; set; }

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
    }
}