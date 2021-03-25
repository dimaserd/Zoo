namespace Tms.Logic.Models
{
    public class DayTaskCommentModel
    {
        public string Id { get; set; }

        public string Comment { get; set; }

        public UserFullNameEmailAndAvatarModel Author { get; set; }
    }
}