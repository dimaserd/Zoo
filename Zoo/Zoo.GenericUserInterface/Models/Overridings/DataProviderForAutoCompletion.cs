using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Провайдер данных для автокомплита
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class DataProviderForAutoCompletion<TItem> : IDataProviderForAutoCompletion
    {
        /// <summary>
        /// Функция для получения данных
        /// </summary>
        /// <param name="typedText"></param>
        /// <returns></returns>
        public abstract Task<AutoCompleteSuggestionData<TItem>[]> GetData(string typedText);

        /// <summary>
        /// Реализация с перекладкой. Используется внутри библиотекой
        /// </summary>
        /// <param name="typedText"></param>
        /// <returns></returns>
        public async Task<AutoCompleteSuggestion[]> GetSuggestionsData(string typedText)
        {
            var data = await GetData(typedText);

            return data.Select(x => x.ToAutoCompleteSuggestion()).ToArray();
        }
    }
}