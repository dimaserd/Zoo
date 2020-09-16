using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.Abstractions
{
    /// <summary>
    /// Абстракция над js обработчиком
    /// </summary>
    public interface IJsWorker
    {
        /// <summary>
        /// Получить локументацию
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        JsWorkerDocumentation JsWorkerDocs(JsWorkerBuilder builder);
    }
}