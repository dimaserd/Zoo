using System.Collections.Generic;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Данные для построения выпадающего списка
    /// </summary>
    public class DropDownListData
    {
        /// <summary>
        /// Значения для выпадающего списка
        /// </summary>
        public List<SelectListItem> SelectList { get; set; }

        /// <summary>
        /// Можно ли добавлять новые данные
        /// </summary>
        public bool CanAddNewItem { get; set; }

        /// <summary>
        /// Провайдер данных
        /// </summary>
        internal string DataProviderTypeFullName { get; set; }
    }
}