namespace Clt.Contract.Settings
{
    /// <summary>
    /// Модель описывающая настройки названий системных ролей
    /// </summary>
    public class CltRolesSetting
    {
        /// <summary>
        /// Название роли администратора
        /// </summary>
        public string AdminRoleName { get; set; }

        /// <summary>
        /// Название роли Рута
        /// </summary>
        public string RootRoleName { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CltRolesSetting()
        {
            AdminRoleName = "Admin";
            RootRoleName = "Root";
        }
    }
}