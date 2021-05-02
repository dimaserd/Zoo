namespace Tms.Logic.Models
{
    /// <summary>
    /// Модель описывающая комментраий к заданию
    /// </summary>
    public class DayTaskCommentModel
    {
        /// <summary>
        /// Идентифкатор комментария
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// Идентифкатор автора
        /// </summary>
        public string AuthorId { get; set; }
    }
}