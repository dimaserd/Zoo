using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Models.Definition;

namespace Zoo.GenericUserInterface.Services.Providers
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
            return (await GetData(typedText))
                .Select(x => x.ToAutoCompleteSuggestion())
                .ToArray();
        }
    }
}