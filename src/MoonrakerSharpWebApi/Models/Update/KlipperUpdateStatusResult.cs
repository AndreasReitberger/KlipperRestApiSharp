using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("github_rate_limit")]
        public partial long? GithubRateLimit { get; set; }

        [ObservableProperty]

        [JsonPropertyName("github_requests_remaining")]
        public partial long? GithubRequestsRemaining { get; set; }

        [ObservableProperty]

        [JsonPropertyName("github_limit_reset_time")]
        public partial long? GithubLimitResetTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("version_info")]
        public partial Dictionary<string, KlipperUpdateVersionInfo> VersionInfo { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("busy")]
        public partial bool Busy { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
