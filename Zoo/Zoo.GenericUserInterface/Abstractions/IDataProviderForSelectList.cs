using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Abstractions
{
    /// <summary>
    /// Провайдер данных для выпадающего списка
    /// </summary>
    public interface IDataProviderForSelectList
    {
        /// <summary>
        /// Получить данные для выпадющего списка
        /// </summary>
        /// <returns></returns>
        Task<SelectListItem[]> GetSelectListItems();
    }
}