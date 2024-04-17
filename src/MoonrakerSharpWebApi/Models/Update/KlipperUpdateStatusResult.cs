using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("github_rate_limit")]
        public long? githubRateLimit;

        [ObservableProperty]
        [property: JsonProperty("github_requests_remaining")]
        public long? githubRequestsRemaining;

        [ObservableProperty]
        [property: JsonProperty("github_limit_reset_time")]
        public long? githubLimitResetTime;

        [ObservableProperty]
        [property: JsonProperty("version_info")]
        public Dictionary<string, KlipperUpdateVersionInfo> versionInfo = [];

        [ObservableProperty]
        [property: JsonProperty("busy")]
        public bool busy;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
