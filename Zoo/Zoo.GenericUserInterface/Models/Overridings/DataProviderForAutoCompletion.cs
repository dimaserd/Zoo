using System.Threading.Tasks;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    public abstract class DataProviderForAutoCompletion<TItem>
    {
        public abstract Task<AutoCompleteSuggestionData<TItem>[]> GetData(string typedText);
    }
}