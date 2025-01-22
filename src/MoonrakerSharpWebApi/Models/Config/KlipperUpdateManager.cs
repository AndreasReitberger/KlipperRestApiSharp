using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManager : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("enable_auto_refresh")]
        public partial bool EnableAutoRefresh { get; set; }

        [ObservableProperty]
        
        [JsonProperty("channel")]
        public partial string Channel { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("enable_repo_debug")]
        public partial bool EnableRepoDebug { get; set; }

        [ObservableProperty]
        
        [JsonProperty("enable_system_updates")]
        public partial bool EnableSystemUpdates { get; set; }

        [ObservableProperty]
        
        [JsonProperty("type")]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("client_repo")]
        public partial object? ClientRepo { get; set; }

        [ObservableProperty]
        
        [JsonProperty("client_path")]
        public partial object? ClientPath { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }

}
