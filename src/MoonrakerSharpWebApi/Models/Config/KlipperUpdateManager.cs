using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManager : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("enable_auto_refresh")]
        bool enableAutoRefresh;

        [ObservableProperty]
        [property: JsonProperty("channel")]
        string channel = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("enable_repo_debug")]
        bool enableRepoDebug;

        [ObservableProperty]
        [property: JsonProperty("enable_system_updates")]
        bool enableSystemUpdates;

        [ObservableProperty]
        [property: JsonProperty("type")]
        string type = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("client_repo")]
        object? clientRepo;

        [ObservableProperty]
        [property: JsonProperty("client_path")]
        object? clientPath;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }

}
