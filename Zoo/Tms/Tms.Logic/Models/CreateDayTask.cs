﻿using System;

namespace Tms.Logic.Models
{
    /// <summary>
    /// Создать или обновить задание
    /// </summary>
    public class CreateOrUpdateDayTask
    {
        /// <summary>
        /// Идентификатор задания
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// День на которое назначено задание
        /// </summary>
        public DateTime TaskDate { get; set; }

        /// <summary>
        /// Описание задания
        /// </summary>
        public string TaskText { get; set; }

        /// <summary>
        /// Название задания
        /// </summary>
        public string TaskTitle { get; set; }

        /// <summary>
        /// Идентификатор исполнителя
        /// </summary>
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