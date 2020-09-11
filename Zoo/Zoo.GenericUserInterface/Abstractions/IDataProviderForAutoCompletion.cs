using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models.Overridings;

namespace Zoo.GenericUserInterface.Abstractions
{
    public interface IDataProviderForAutoCompletion<TItem>
    {
        Task<AutoCompleteSuggestionData<TItem>[]> GetData(string typedText);
    }
}