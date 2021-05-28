using System.ComponentModel.DataAnnotations;
using Tms.Logic.Resources;

namespace Tms.Logic.Models.Comments
{
    /// <summary>
    /// Модель для обновления комментария
    /// </summary>
    public class UpdateDayTaskComment
    {
        /// <summary>
        /// Идентификатор комментария
        /// </summary>
        public string DayTaskCommentId { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(TaskerResource), ErrorMessageResourceName = nameof(TaskerResource.CommentCantBeEmpty))]
        public string Comment { get; set; }
    }
}