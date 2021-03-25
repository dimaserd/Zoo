using System.ComponentModel.DataAnnotations;

namespace Tms.Logic.Models
{
    public class UserFullNameEmailAndAvatarModel
    {
        public string Id { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Идентификатор аватара")]
        public int? AvatarFileId { get; set; }
    }
}