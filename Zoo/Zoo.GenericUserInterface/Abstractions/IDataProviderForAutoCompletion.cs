using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models.Overridings;

namespace Zoo.GenericUserInterface.Abstractions
{
    /// <summary>
    /// Провайдер данных для автокомплита
    /// </summary>
    public interface IDataProviderForAutoCompletion
    {
        /// <summary>
        /// Получить данные в зависимости от подсказки
        /// </summary>
        /// <param name="typedText"></param>
        /// <returns></returns>
        Task<AutoCompleteSuggestion[]> GetSuggestionsData(string typedText);
    }
}