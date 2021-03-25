using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Common
{
    /// <summary>
    /// Пользователь с полями
    /// </summary>
    public class UserWithNameAndEmailAvatarModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Display(Name = "Имя")]
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор аватара
        /// </summary>
        [Display(Name = "Идентификатор аватара")]
        public int? AvatarFileId { get; set; }
    }
}