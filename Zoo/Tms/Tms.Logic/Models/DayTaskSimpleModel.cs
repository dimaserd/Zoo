using System;

namespace Tms.Logic.Models
{
    /// <summary>
    /// Модель задания на день
    /// </summary>
    public class DayTaskSimpleModel
    {
        /// <summary>
        /// Контструктор
        /// </summary>
        /// <param name="model"></param>
        /// <param name="author"></param>
        /// <param name="assignee"></param>
        internal DayTaskSimpleModel(DayTaskModelWithNoUsersJustIds model, 
            UserFullNameEmailAndAvatarModel author, 
            UserFullNameEmailAndAvatarModel assignee)
        {
            Id = model.Id;
            TaskDate = model.TaskDate;
            TaskText = model.TaskText;
            TaskTitle = model.TaskTitle;
            FinishDate = model.FinishDate;
            TaskTarget = model.TaskTarget;
            TaskReview = model.TaskReview;
            TaskComment = model.TaskComment;
            Author = author;
            AssigneeUser = assignee;
        }

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
        public UserFullNameEmailAndAvatarModel Author { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public UserFullNameEmailAndAvatarModel AssigneeUser { get; set; }
    }
}