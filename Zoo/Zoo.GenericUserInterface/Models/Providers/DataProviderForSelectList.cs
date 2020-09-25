using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models.Overridings;

namespace Zoo.GenericUserInterface.Models.Providers
{
    /// <summary>
    /// Провайдер данных для выпадающего списка
    /// </summary>
    /// <typeparam name="TITem"></typeparam>
    public abstract class DataProviderForSelectList<TITem> : IDataProviderForSelectList
    {
        /// <summary>
        /// Функция для получения данных
        /// </summary>
        /// <returns></returns>
        public abstract Task<SelectListItemData<TITem>[]> GetData();

        /// <summary>
        /// Реализация с перекладкой. Используется внутри библиотекой
        /// </summary>
        /// <returns></returns>
        public async Task<SelectListItem[]> GetSelectListItems()
        {
            return (await GetData())
                .Select(GenericUserInterfaceModelBuilderExtensions.ToSelectListItem)
                .ToArray();
        }
    }
}