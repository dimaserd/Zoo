using System;
using System.Net.Http;
using System.Threading.Tasks;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Http;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Дефолтный исполнитель Http запросов
    /// </summary>
    public class ServerJsHttpClient : IServerJsHttpClient
    {
        HttpClient HttpClient { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="httpClient"></param>
        public ServerJsHttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        private async Task<HttpResponseRecord> CallInner(HttpResponseRecord result, Func<HttpClient, Task<HttpResponseMessage>> funcOnClient)
        {
            HttpResponseMessage httpResult;

            try
            {
                httpResult = await funcOnClient(HttpClient);
            }
            catch (Exception ex)
            {
                result.ExcepionData = ExcepionData.Create(ex);
                return result;
            }

            result.ResponseStatusCode = (int)httpResult.StatusCode;
            result.Response = await httpResult.Content.ReadAsStringAsync();

            return result;
        }

        /// <inheritdoc />
        public Task<HttpResponseRecord> PostAsync<TRequest>(string hostUrl, string hostName, string path, TRequest request)
        {
            var json = ZooSerializer.Serialize(request);

            var url = $"{hostUrl}{path}";

            var result = new HttpResponseRecord
            {
                HostName = hostName,
                HostUrl = hostUrl,
                RequestUrl = url,
                Request = json,
                RequestMethod = "POST"
            };

            return CallInner(result, httpClient => HttpClient.PostAsync(url, new StringContent(json)));
        }

        /// <inheritdoc />
        public Task<HttpResponseRecord> GetAsync(string hostUrl, string hostName, string path)
        {
            var url = $"{hostUrl}{path}";

            var result = new HttpResponseRecord
            {
                HostName = hostName,
                HostUrl = hostUrl,
                RequestUrl = url,
                RequestMethod = "GET"
            };

            return CallInner(result, httpClient => HttpClient.GetAsync(url));
        }
    }
}