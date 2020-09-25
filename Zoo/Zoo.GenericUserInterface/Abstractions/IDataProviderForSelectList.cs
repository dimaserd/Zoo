using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Abstractions
{
    internal interface IDataProviderForSelectList
    {
        Task<SelectListItem[]> GetSelectListItems();
    }
}