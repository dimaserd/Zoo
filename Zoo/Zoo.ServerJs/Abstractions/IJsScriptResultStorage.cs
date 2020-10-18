using System;
using System.Threading.Tasks;
using Zoo.ServerJs.Models;

namespace Zoo.ServerJs.Abstractions
{
    /// <summary>
    /// Хранилище для выполненных скриптов
    /// </summary>
    public interface IJsScriptResultStorage
    {
        /// <summary>
        /// Добавить результат выполнения скрипта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddScriptResult(JsScriptExecutedResult model);
        
        /// <summary>
        /// Найти по идентификатору
        /// </summary>
        /// <param name="scriptId"></param>
        /// <returns></returns>
        Task GetScriptResultById(Guid scriptId);
        
        /// <summary>
        /// Искать скрипты
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<JsScriptExecutedResult[]> GetScriptResults(SearchScriptResults model);
    }
}
