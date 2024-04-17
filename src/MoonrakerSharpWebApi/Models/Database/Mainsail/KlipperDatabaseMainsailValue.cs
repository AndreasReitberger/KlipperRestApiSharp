using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValue : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("dashboard")]
        KlipperDatabaseMainsailValueDashboard? dashboard;

        [ObservableProperty]
        [property: JsonProperty("general")]
        KlipperDatabaseMainsailValueGeneral? general;

        [ObservableProperty]
        [property: JsonProperty("heightmap")]
        KlipperDatabaseMainsailValueHeightmapSettings? heightmap;

        [ObservableProperty]
        [property: JsonProperty("init")]
        bool init;

        [ObservableProperty]
        [property: JsonProperty("presets")]
        List<KlipperDatabaseMainsailValuePreset> presets = [];

        [ObservableProperty]
        [property: JsonProperty("remote_printers")]
        List<object> pemotePrinters = [];

        [ObservableProperty]
        [property: JsonProperty("settings")]
        KlipperDatabaseMainsailValueSettings? settings;

        [ObservableProperty]
        [property: JsonProperty("webcam")]
        KlipperDatabaseMainsailValueWebcam? webcam;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
