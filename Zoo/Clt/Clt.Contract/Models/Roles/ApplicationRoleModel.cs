namespace Clt.Contract.Models.Roles
{
    /// <summary>
    /// Модель роли пользователя
    /// </summary>
    public class ApplicationRoleModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Название для отображения
        /// </summary>
        public string DisplayRoleName { get; set; }
    }
}