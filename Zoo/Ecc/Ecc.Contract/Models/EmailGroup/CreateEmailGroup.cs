using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.EmailGroup
{
    /// <summary>
    /// Создать группу эмейлов
    /// </summary>
    public class CreateEmailGroup
    {
        /// <summary>
        /// Название группы
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать название группы")]
        public string Name { get; set; }
    }
}