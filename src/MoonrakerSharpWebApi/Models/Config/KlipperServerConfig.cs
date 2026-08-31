using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("server")]
        public partial KlipperServer? Server { get; set; }

        [ObservableProperty]
        [JsonPropertyName("authorization")]
        public partial KlipperAuthorization? Authorization { get; set; }

        [ObservableProperty]
        [JsonPropertyName("octoprint_compat")]
        public partial object? OctoprintCompat { get; set; }

        [ObservableProperty]
        [JsonPropertyName("history")]
        public partial object? History { get; set; }

        [ObservableProperty]
        [JsonPropertyName("update_manager")]
        public partial KlipperUpdateManager? UpdateManager { get; set; }

        [ObservableProperty]
        [JsonPropertyName("update_manager moonraker")]
        public partial KlipperUpdateManagerKlipperClass? UpdateManagerMoonraker { get; set; }

        [ObservableProperty]
        [JsonPropertyName("update_manager klipper")]
        public partial KlipperUpdateManagerKlipperClass? UpdateManagerKlipper { get; set; }

        [ObservableProperty]
        [JsonPropertyName("update_manager mainsail")]
        public partial KlipperUpdateManagerMainsail? UpdateManagerMainsail { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
