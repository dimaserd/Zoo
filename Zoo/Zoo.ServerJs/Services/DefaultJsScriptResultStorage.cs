using System;
using System.Threading.Tasks;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Дефолтная реализация хранилища без хранилища
    /// </summary>
    public class DefaultJsScriptResultStorage : IJsScriptResultStorage
    {
        readonly string ErrorMessage = "Операции поиска не реализованы в дефолтном хранилище";
        
        /// <inheritdoc />
        public Task AddScriptResult(JsScriptExecutedResult model)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task GetScriptResultById(Guid scriptId)
        {
            throw new NotImplementedException(ErrorMessage);
        }

        /// <inheritdoc />
        public Task<JsScriptExecutedResult[]> GetScriptResults(SearchScriptResults model)
        {
            throw new NotImplementedException(ErrorMessage);
        }
    }
}