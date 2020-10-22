using System.Threading.Tasks;
using Zoo.ServerJs.Models.Http;

namespace Zoo.ServerJs.Abstractions
{
    /// <summary>
    /// Http клиент для js исполнителя
    /// </summary>
    public interface IServerJsHttpClient
    {
        /// <summary>
        /// Совершить Post запрос
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="hostUrl"></param>
        /// <param name="hostName"></param>
        /// <param name="path"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<HttpResponseRecord> PostAsync<TRequest>(string hostUrl, string hostName, string path, TRequest request);
        
        /// <summary>
        /// Совершить Get запрос
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="hostName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<HttpResponseRecord> GetAsync(string hostUrl, string hostName, string path);
    }
}