using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.EmailGroup
{
    public class CreateEmailGroup
    {
        [Required(ErrorMessage = "Необходимо указать название группы")]
        public string Name { get; set; }
    }
}