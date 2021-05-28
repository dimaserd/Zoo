using System.ComponentModel.DataAnnotations;
using Tms.Logic.Resources;

namespace Tms.Logic.Models.Comments
{
    /// <summary>
    /// Комментировать задание
    /// </summary>
    public class CommentDayTask
    {
        /// <summary>
        /// Идентификатор задания
        /// </summary>
        public string DayTaskId { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(TaskerResource), ErrorMessageResourceName = nameof(TaskerResource.CommentCantBeEmpty))]
        public string Comment { get; set; }
    }
}