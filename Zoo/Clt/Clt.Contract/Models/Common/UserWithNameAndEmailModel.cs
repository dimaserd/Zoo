using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Common
{
    public class UserWithNameAndEmailAvatarModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Идентификатор аватара")]
        public int? AvatarFileId { get; set; }
    }
}