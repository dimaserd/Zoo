using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ecc.Logic.Models.IntegratedApps
{
    /// <summary>
    /// Отправить уведомление через OneSignal
    /// </summary>
    public class SendNotificationViaOneSignal
    {
        /// <summary>
        /// Идентифкатор приложения
        /// </summary>
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        /// <summary>
        /// Вложенные сегменты
        /// </summary>
        [JsonProperty("included_segments")]
        public string[] IncludedSegments { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string, string> Data { get; set; }

        /// <summary>
        /// Содержимое
        /// </summary>
        [JsonProperty("contents")]
        public Dictionary<string, string> Contents { get; set; }

        /// <summary>
        /// Заголовки
        /// </summary>
        [JsonProperty("headings")]
        public Dictionary<string, string> Headings { get; set; }

        /// <summary>
        /// Включенные идентификаторы
        /// </summary>
        [JsonProperty("include_player_ids")]
        public string[] IncludePlayerIds { get; set; }
    }
}