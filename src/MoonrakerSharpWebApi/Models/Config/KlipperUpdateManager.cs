using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManager : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("enable_auto_refresh")]
        public partial bool EnableAutoRefresh { get; set; }

        [ObservableProperty]
        [JsonPropertyName("channel")]
        public partial string Channel { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("enable_repo_debug")]
        public partial bool EnableRepoDebug { get; set; }

        [ObservableProperty]
        [JsonPropertyName("enable_system_updates")]
        public partial bool EnableSystemUpdates { get; set; }

        [ObservableProperty]
        [JsonPropertyName("type")]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("client_repo")]
        public partial object? ClientRepo { get; set; }

        [ObservableProperty]
        [JsonPropertyName("client_path")]
        public partial object? ClientPath { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }

}
