using Ecc.Logic.Models.IntegratedApps;
using Ecc.Model.Entities.IntegratedApps;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ecc.Logic.Clients
{
    /// <summary>
    /// Клиент для взаимодействия с OneSignal Api
    /// </summary>
    public class OneSignalClient
    {
        private HttpClient HttpClient { get; }
        private ILogger<OneSignalClient> Logger { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        public OneSignalClient(HttpClient httpClient, ILogger<OneSignalClient> logger)
        {
            HttpClient = httpClient;
            Logger = logger;
        }

        /// <summary>
        /// Отправить уведомление для Ios приложения
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public async Task GetSendNotificationTaskForIosApp(IntegratedAppUserSetting setting, SendUserNotificationViaApplication reqModel)
        {
            var model = new SendNotificationViaOneSignal
            {
                AppId = setting.App.Uid,
                Data = new Dictionary<string, string>
                {
                    ["foo"] = "bar"
                },
                Contents = new Dictionary<string, string>
                {
                    ["en"] = "English Message",
                    ["ru"] = reqModel.Text
                },
                Headings = new Dictionary<string, string>
                {
                    ["en"] = "111",
                    ["ru"] = reqModel.Title
                },
                IncludePlayerIds = new[]
                {
                    setting.UserUidInApp
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://onesignal.com/api/v1/notifications")
            {
                Content = CreateRequestStringContent(model)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Authorization", $"Basic {setting.App.ConfigurationJson}");
            
            try
            {
                var response = await HttpClient.SendAsync(request);
            }
            catch (Exception error)
            {
                Logger.LogError(error, "IntegratedAppWorker.GetSendNotificationTaskForIosApp.OnException");
            }
        }

        private static StringContent CreateRequestStringContent(object requestModel)
        {
            return new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
        }
    }
}
