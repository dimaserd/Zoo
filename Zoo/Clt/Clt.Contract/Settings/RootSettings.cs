namespace Clt.Contract.Settings
{
    /// <summary>
    /// Настройки прав для рута
    /// </summary>
    public class RootSettings
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RootSettings()
        {
            RootEmail = "root@mail.com";
            RootPassword = "RootPassword@1234";
            UserRemovingEnabled = true;
        }

        /// <summary>
        /// Электронный адрес рута
        /// </summary>
        public string RootEmail { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string RootPassword { get; set; }

        /// <summary>
        /// Разрешено ли руту удалять пользователей
        /// </summary>
        public bool UserRemovingEnabled { get; set; }
    }
}