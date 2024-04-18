using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManager : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_auto_refresh")]
        bool enableAutoRefresh;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("channel")]
        string channel = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_repo_debug")]
        bool enableRepoDebug;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_system_updates")]
        bool enableSystemUpdates;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("type")]
        string type = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("client_repo")]
        object? clientRepo;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("client_path")]
        object? clientPath;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }

}
