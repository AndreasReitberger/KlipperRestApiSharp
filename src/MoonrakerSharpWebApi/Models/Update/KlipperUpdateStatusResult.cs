using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateStatusResult
    {
        #region Properties
        [JsonProperty("github_rate_limit")]
        public long? GithubRateLimit { get; set; }

        [JsonProperty("github_requests_remaining")]
        public long? GithubRequestsRemaining { get; set; }

        [JsonProperty("github_limit_reset_time")]
        public long? GithubLimitResetTime { get; set; }

        [JsonProperty("version_info")]
        public Dictionary<string, KlipperUpdateVersionInfo> VersionInfo { get; set; } = [];

        [JsonProperty("busy")]
        public bool Busy { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
