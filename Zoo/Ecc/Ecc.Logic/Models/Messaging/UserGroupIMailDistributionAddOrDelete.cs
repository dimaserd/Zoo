namespace Ecc.Logic.Models.Messaging
{
    /// <summary>
    /// Модель добавления или удаления одной группы пользователя из рассылки
    /// </summary>
    public class UserGroupIMailDistributionAddOrDelete
    {
        /// <summary>
        /// Идентификатор группы пользователей
        /// </summary>
        public string UserGroupId { get; set; }

        /// <summary>
        /// Если добавить в рассылку то значение равно true, если удалить значить false
        /// </summary>
        public bool AddOrDelete { get; set; }
    }
}