using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("server")]
        KlipperServer? server;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("authorization")]
        KlipperAuthorization? authorization;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("octoprint_compat")]
        object? octoprintCompat;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("history")]
        object? history;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("update_manager")]
        KlipperUpdateManager? updateManager;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("update_manager moonraker")]
        KlipperUpdateManagerKlipperClass? updateManagerMoonraker;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("update_manager klipper")]
        KlipperUpdateManagerKlipperClass? updateManagerKlipper;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("update_manager mainsail")]
        KlipperUpdateManagerMainsail? updateManagerMainsail;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
