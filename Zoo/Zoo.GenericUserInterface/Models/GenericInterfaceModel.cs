using System.Collections.Generic;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Интерфейс
    /// </summary>
    public class GenericInterfaceModel
    {
        /// <summary>
        /// Префикс для построения модели
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Блоки для свойств
        /// </summary>
        public List<UserInterfaceBlock> Blocks { get; set; }
    }
}