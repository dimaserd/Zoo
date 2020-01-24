using Zoo.ServerJs.Models;

namespace Zoo.ServerJs.Abstractions
{
    /// <summary>
    /// Абстракция над js обработчиком
    /// </summary>
    public interface IJsWorker
    {
        /// <summary>
        /// Получить документацию
        /// </summary>
        /// <returns></returns>
        JsWorkerDocumentation JsWorkerDocs();
    }
}