using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.EmailGroup
{
    /// <summary>
    /// Добавить эмейлы в группу
    /// </summary>
    public class AddEmailsToEmailGroup
    {
        /// <summary>
        /// Идентификатор группы эмейлов
        /// </summary>
        public string EmailGroupId { get; set; }

        /// <summary>
        /// Эмейлы
        /// </summary>
        [Required(ErrorMessage = "Нужно указать Email адреса")]
        public List<string> Emails { get; set; }
    }
}