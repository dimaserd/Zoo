using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    public abstract class DataProviderForAutoCompletion<TItem> : IDataProviderForAutoCompletion
    {
        public abstract Task<AutoCompleteSuggestionData<TItem>[]> GetData(string typedText);

        public async Task<AutoCompleteSuggestion[]> GetSuggestionsData(string typedText)
        {
            var data = await GetData(typedText);

            return data.Select(x => x.ToAutoCompleteSuggestion()).ToArray();
        }
    }
}