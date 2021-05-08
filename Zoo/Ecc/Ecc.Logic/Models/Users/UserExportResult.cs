namespace Ecc.Logic.Models.Users
{
    /// <summary>
    /// Модель описывающая результат экспортирования пользователей
    /// </summary>
    public class UserExportResult
    {
        /// <summary>
        /// Экспортировано пользователей
        /// </summary>
        public int UsersExported { get; set; }

        /// <summary>
        /// Всего пользователей в хранилище
        /// </summary>
        public int TotalUsers { get; set; }
    }
}