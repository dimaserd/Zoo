using System.Collections.Generic;

namespace Ecc.Logic.Models
{
    /// <summary>
    /// Описание функции текстовой замены
    /// </summary>
    public class EccTextFunc
    {
        /// <summary>
        /// Название функции
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Аргументы
        /// </summary>
        public List<string> Args { get; set; }
    }
}