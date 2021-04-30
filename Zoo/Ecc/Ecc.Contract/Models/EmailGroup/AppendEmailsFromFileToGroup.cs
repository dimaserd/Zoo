using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.EmailGroup
{
    /// <summary>
    /// Добавить эмейлы из файла в группу эмейлов
    /// </summary>
    public class AppendEmailsFromFileToGroup
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать идентификатор группы")]
        public string EmailGroupId { get; set; }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать название файла")]
        public string FilePath { get; set; }
    }
}