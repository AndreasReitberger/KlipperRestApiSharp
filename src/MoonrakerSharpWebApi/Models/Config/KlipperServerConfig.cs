using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("server")]
        KlipperServer? server;

        [ObservableProperty]
        [property: JsonProperty("authorization")]
        KlipperAuthorization? authorization;

        [ObservableProperty]
        [property: JsonProperty("octoprint_compat")]
        object? octoprintCompat;

        [ObservableProperty]
        [property: JsonProperty("history")]
        object? history;

        [ObservableProperty]
        [property: JsonProperty("update_manager")]
        KlipperUpdateManager? updateManager;

        [ObservableProperty]
        [property: JsonProperty("update_manager moonraker")]
        KlipperUpdateManagerKlipperClass? updateManagerMoonraker;

        [ObservableProperty]
        [property: JsonProperty("update_manager klipper")]
        KlipperUpdateManagerKlipperClass? updateManagerKlipper;

        [ObservableProperty]
        [property: JsonProperty("update_manager mainsail")]
        KlipperUpdateManagerMainsail? updateManagerMainsail;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
