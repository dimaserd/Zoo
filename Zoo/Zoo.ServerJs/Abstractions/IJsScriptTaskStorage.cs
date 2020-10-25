using Croco.Core.Abstractions.Models;
using Croco.Core.Abstractions.Models.Search;
using System;
using System.Threading.Tasks;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Task;

namespace Zoo.ServerJs.Abstractions
{
    /// <summary>
    /// Хранилище для скриптов
    /// </summary>
    public interface IJsScriptTaskStorage
    {
        /// <summary>
        /// Добавить задание
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddTask(AddJsTask model);

        /// <summary>
        /// Добавить результат исполненного скрипта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddResult(JsScriptExecutedResult model);

        /// <summary>
        /// Получить задание по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<JsTaskModel> GetTask(Guid id);

        /// <summary>
        /// Отменить задание
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseApiResponse> CancelTask(Guid id);

        /// <summary>
        /// Искать скрипты
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<GetListResult<JsTaskModel>> SearchTasks(SearchJsTasks model);
    }
}
