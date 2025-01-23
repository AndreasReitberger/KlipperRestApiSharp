using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("server")]
        public partial KlipperServer? Server { get; set; }

        [ObservableProperty]

        [JsonProperty("authorization")]
        public partial KlipperAuthorization? Authorization { get; set; }

        [ObservableProperty]

        [JsonProperty("octoprint_compat")]
        public partial object? OctoprintCompat { get; set; }

        [ObservableProperty]

        [JsonProperty("history")]
        public partial object? History { get; set; }

        [ObservableProperty]

        [JsonProperty("update_manager")]
        public partial KlipperUpdateManager? UpdateManager { get; set; }

        [ObservableProperty]

        [JsonProperty("update_manager moonraker")]
        public partial KlipperUpdateManagerKlipperClass? UpdateManagerMoonraker { get; set; }

        [ObservableProperty]

        [JsonProperty("update_manager klipper")]
        public partial KlipperUpdateManagerKlipperClass? UpdateManagerKlipper { get; set; }

        [ObservableProperty]

        [JsonProperty("update_manager mainsail")]
        public partial KlipperUpdateManagerMainsail? UpdateManagerMainsail { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
