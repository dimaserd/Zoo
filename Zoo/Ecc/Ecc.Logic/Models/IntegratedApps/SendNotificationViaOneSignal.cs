using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ecc.Logic.Models.IntegratedApps
{
    public class SendNotificationViaOneSignal
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("included_segments")]
        public string[] IncludedSegments { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, string> Data { get; set; }

        [JsonProperty("contents")]
        public Dictionary<string, string> Contents { get; set; }

        [JsonProperty("headings")]
        public Dictionary<string, string> Headings { get; set; }

        [JsonProperty("include_player_ids")]
        public string[] IncludePlayerIds { get; set; }
    }
}