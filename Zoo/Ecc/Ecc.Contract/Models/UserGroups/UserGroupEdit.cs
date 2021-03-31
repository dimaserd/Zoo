namespace Ecc.Contract.Models.UserGroups
{
    /// <summary>
    /// Редактирование группы пользователей
    /// </summary>
    public class UserGroupEdit
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}