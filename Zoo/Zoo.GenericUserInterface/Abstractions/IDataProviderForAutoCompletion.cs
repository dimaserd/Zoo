using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models.Overridings;

namespace Zoo.GenericUserInterface.Abstractions
{
    public interface IDataProviderForAutoCompletion
    {
        Task<AutoCompleteSuggestion[]> GetSuggestionsData(string typedText);
    }
}