using System.Net.Http;
using Zoo.ServerJs.Abstractions;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Дефолтный Http клиент
    /// </summary>
    public class DefaultHttpClientProvider : IServerJsHttpClientProvider
    {
        readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Получить Http клиента
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClient()
        {
            return _client;
        }
    }
}