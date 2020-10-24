using System;
using System.Threading.Tasks;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.OpenApi;

namespace Zoo.ServerJs.Extensions
{
    internal static class IServerJsHttpClientExtensions
    {
        internal static async Task<RemoteJsOpenApiDocs> GetRemoteDocsViaHttpRequest(this IServerJsHttpClient httpClient, RemoteJsOpenApi remoteApi)
        {
            var response = await httpClient.GetAsync(remoteApi.HostUrl, remoteApi.Name, HttpPaths.GetDocs);

            var docsRecord = new RemoteJsOpenApiDocs
            {
                Description = remoteApi,
                ReceivingException = response.ExcepionData,
                DocsReceivedOnUtc = DateTime.UtcNow
            };

            try
            {
                if (response.IsSuccessfull())
                {
                    docsRecord.Docs = response.GetResult<JsOpenApiDocs>();
                    docsRecord.IsDocsReceived = true;
                }
                else
                {
                    docsRecord.IsDocsReceived = false;
                    docsRecord.DocsReceivedOnUtc = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                docsRecord.IsDocsReceived = false;
                docsRecord.ReceivingException = new ExcepionData
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }

            return docsRecord;
        }

    }
}
