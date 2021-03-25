using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.EmailGroup
{
    public class AppendEmailsFromFileToGroup
    {
        [Required(ErrorMessage = "Необходимо указать идентификатор группы")]
        public string EmailGroupId { get; set; }

        [Required(ErrorMessage = "Необходимо указать название файла")]
        public string FilePath { get; set; }
    }
}