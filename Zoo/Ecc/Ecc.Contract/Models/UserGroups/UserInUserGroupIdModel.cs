namespace Ecc.Contract.Models.UserGroups
{
    /// <summary>
    /// Перевязка пользователя с группой
    /// </summary>
    public class UserInUserGroupIdModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public string UserGroupId { get; set; }
    }
}