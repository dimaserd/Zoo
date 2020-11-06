using Croco.Core.Contract.Models;
using Croco.Core.Contract.Models.Search;
using System;
using System.Threading.Tasks;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Task;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Дефолтная реализация хранилища без хранилища
    /// </summary>
    public class DefaultJsScriptResultStorage : IJsScriptTaskStorage
    {
        readonly string ErrorMessage = "Операции поиска не реализованы в дефолтном хранилище";

        /// <inheritdoc />
        public Task AddResult(JsScriptExecutedResult model)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task AddTask(AddJsTask model)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<BaseApiResponse> CancelTask(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<JsTaskModel> GetTask(Guid id)
        {
            throw new NotImplementedException(ErrorMessage);
        }

        /// <inheritdoc />
        public Task<GetListResult<JsTaskModel>> SearchTasks(SearchJsTasks model)
        {
            throw new NotImplementedException(ErrorMessage);
        }
    }
}