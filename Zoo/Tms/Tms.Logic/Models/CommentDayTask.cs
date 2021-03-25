using System.ComponentModel.DataAnnotations;
using Tms.Logic.Resources;

namespace Tms.Logic.Models
{
    public class CommentDayTask
    {
        public string DayTaskId { get; set; }

        [Required(ErrorMessageResourceType = typeof(TaskerResource), ErrorMessageResourceName = nameof(TaskerResource.CommentCantBeEmpty))]
        public string Comment { get; set; }
    }
}