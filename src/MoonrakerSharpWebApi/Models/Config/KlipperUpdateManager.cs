using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManager
    {
        #region Properties
        [JsonProperty("enable_auto_refresh")]
        public bool EnableAutoRefresh { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("enable_repo_debug")]
        public bool EnableRepoDebug { get; set; }

        [JsonProperty("enable_system_updates")]
        public bool EnableSystemUpdates { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("client_repo")]
        public object ClientRepo { get; set; }

        [JsonProperty("client_path")]
        public object ClientPath { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }

}
