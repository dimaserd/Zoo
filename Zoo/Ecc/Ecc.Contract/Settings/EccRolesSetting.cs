namespace Ecc.Contract.Settings
{
    /// <summary>
    /// Настройка ролей для контекста Ecc
    /// </summary>
    public class EccRolesSetting
    {
        /// <summary>
        /// Название роли
        /// </summary>
        public string AdminRoleName { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public EccRolesSetting()
        {
            AdminRoleName = "Admin";
        }
    }
}