using System.Net.Http;

namespace Zoo.ServerJs.Abstractions
{
    /// <summary>
    /// Провайдер Http клиента для запросов в удаленные Апи
    /// </summary>
    public interface IServerJsHttpClientProvider
    {
        /// <summary>
        /// Получить Http клиента
        /// </summary>
        /// <returns></returns>
        HttpClient GetHttpClient();
    }
}