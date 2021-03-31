namespace Ecc.Contract.Models.UserGroups
{
    /// <summary>
    /// Создание группы
    /// </summary>
    public class UserGroupCreate
    {
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