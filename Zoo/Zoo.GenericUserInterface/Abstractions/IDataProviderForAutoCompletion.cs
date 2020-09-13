using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models.Overridings;

namespace Zoo.GenericUserInterface.Abstractions
{
    internal interface IDataProviderForAutoCompletion
    {
        Task<AutoCompleteSuggestion[]> GetSuggestionsData(string typedText);
    }
}