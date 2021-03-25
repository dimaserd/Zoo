namespace Clt.Logic.Models.Users
{
    /// <summary>
    /// Модель активации пользователя
    /// </summary>
    public class UserActivation
    {
        /// <summary>
        /// Если true, значит пользователь деактивирован в системе
        /// </summary>
        public bool DeActivated { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }
    }
}