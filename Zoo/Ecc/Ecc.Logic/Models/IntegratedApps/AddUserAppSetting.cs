using System.ComponentModel.DataAnnotations;

namespace Ecc.Logic.Models.IntegratedApps
{
    public class AddUserAppSetting
    {
        [Required(ErrorMessage = "Необходимо указать идентификатор приложения")]
        public string AppUId { get; set; }

        [Required(ErrorMessage = "Необходимо указать идентификатор пользователя в данном приложении")]
        public string UserUidInApp { get; set; }
    }
}