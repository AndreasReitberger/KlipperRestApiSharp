using System;
using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManagerKlipperClass : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("type")]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("venv_args")]
        public partial string VenvArgs { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("is_system_service")]
        public partial bool IsSystemService { get; set; }

        [ObservableProperty]
        [JsonPropertyName("moved_origin")]
        public partial Uri? MovedOrigin { get; set; }

        [ObservableProperty]
        [JsonPropertyName("origin")]
        public partial Uri? Origin { get; set; }

        [ObservableProperty]
        [JsonPropertyName("primary_branch")]
        public partial string PrimaryBranch { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("enable_node_updates")]
        public partial bool EnableNodeUpdates { get; set; }

        [ObservableProperty]
        [JsonPropertyName("requirements")]
        public partial string Requirements { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("install_script")]
        public partial string InstallScript { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
