using System.ComponentModel.DataAnnotations;

namespace Ecc.Logic.Models.IntegratedApps
{
    /// <summary>
    /// Добавить настройку для пользователя для приложения
    /// </summary>
    public class AddUserAppSetting
    {
        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать идентификатор приложения")]
        public string AppUId { get; set; }

        /// <summary>
        /// Идентификатор пользователя в приложении
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать идентификатор пользователя в данном приложении")]
        public string UserUidInApp { get; set; }
    }
}