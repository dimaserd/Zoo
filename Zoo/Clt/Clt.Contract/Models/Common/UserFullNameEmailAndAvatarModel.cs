using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Common
{
    public class UserFullNameEmailAndAvatarModel : UserWithNameAndEmailAvatarModel
    {
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
    }
}