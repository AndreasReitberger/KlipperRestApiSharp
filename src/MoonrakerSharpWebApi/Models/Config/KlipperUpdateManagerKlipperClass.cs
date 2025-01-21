using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManagerKlipperClass : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("type")]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("venv_args")]
        public partial string VenvArgs { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("is_system_service")]
        public partial bool IsSystemService { get; set; }

        [ObservableProperty]
        
        [JsonProperty("moved_origin")]
        public partial Uri? MovedOrigin { get; set; }

        [ObservableProperty]
        
        [JsonProperty("origin")]
        public partial Uri? Origin { get; set; }

        [ObservableProperty]
        
        [JsonProperty("primary_branch")]
        public partial string PrimaryBranch { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("enable_node_updates")]
        public partial bool EnableNodeUpdates { get; set; }

        [ObservableProperty]
        
        [JsonProperty("requirements")]
        public partial string Requirements { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("install_script")]
        public partial string InstallScript { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
