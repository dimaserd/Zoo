using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    public abstract class DataProviderForAutoCompletion<TItem> : IDataProviderForAutoCompletion<TItem>
    {
        public abstract Task<AutoCompleteSuggestionData<TItem>[]> GetData(string typedText);
    }
}