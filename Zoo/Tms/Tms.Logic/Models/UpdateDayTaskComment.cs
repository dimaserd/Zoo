using System.ComponentModel.DataAnnotations;
using Tms.Logic.Resources;

namespace Tms.Logic.Models
{
    public class UpdateDayTaskComment
    {
        public string DayTaskCommentId { get; set; }

        [Required(ErrorMessageResourceType = typeof(TaskerResource), ErrorMessageResourceName = nameof(TaskerResource.CommentCantBeEmpty))]
        public string Comment { get; set; }
    }
}