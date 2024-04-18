using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValue : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("dashboard")]
        KlipperDatabaseMainsailValueDashboard? dashboard;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("general")]
        KlipperDatabaseMainsailValueGeneral? general;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heightmap")]
        KlipperDatabaseMainsailValueHeightmapSettings? heightmap;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("init")]
        bool init;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("presets")]
        List<KlipperDatabaseMainsailValuePreset> presets = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("remote_printers")]
        List<object> pemotePrinters = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("settings")]
        KlipperDatabaseMainsailValueSettings? settings;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("webcam")]
        KlipperDatabaseMainsailValueWebcam? webcam;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
