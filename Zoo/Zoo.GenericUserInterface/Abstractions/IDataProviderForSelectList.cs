using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Abstractions
{
    public interface IDataProviderForSelectList
    {
        Task<SelectListItem[]> GetSelectListItems();
    }
}