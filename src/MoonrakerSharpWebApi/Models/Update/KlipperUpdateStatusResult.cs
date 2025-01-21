using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("github_rate_limit")]
        public partial long? GithubRateLimit { get; set; }

        [ObservableProperty]
        
        [JsonProperty("github_requests_remaining")]
        public partial long? GithubRequestsRemaining { get; set; }

        [ObservableProperty]
        
        [JsonProperty("github_limit_reset_time")]
        public partial long? GithubLimitResetTime { get; set; }

        [ObservableProperty]
        
        [JsonProperty("version_info")]
        public partial Dictionary<string, KlipperUpdateVersionInfo> VersionInfo { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("busy")]
        public partial bool Busy { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
