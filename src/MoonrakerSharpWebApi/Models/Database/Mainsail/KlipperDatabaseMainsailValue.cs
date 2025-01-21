using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValue : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("dashboard")]
        public partial KlipperDatabaseMainsailValueDashboard? Dashboard { get; set; }

        [ObservableProperty]
        
        [JsonProperty("general")]
        public partial KlipperDatabaseMainsailValueGeneral? General { get; set; }

        [ObservableProperty]
        
        [JsonProperty("heightmap")]
        public partial KlipperDatabaseMainsailValueHeightmapSettings? Heightmap { get; set; }

        [ObservableProperty]
        
        [JsonProperty("init")]
        public partial bool Init { get; set; }

        [ObservableProperty]
        
        [JsonProperty("presets")]
        public partial List<KlipperDatabaseMainsailValuePreset> Presets { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("remote_printers")]
        public partial List<object> PemotePrinters { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("settings")]
        public partial KlipperDatabaseMainsailValueSettings? Settings { get; set; }

        [ObservableProperty]
        
        [JsonProperty("webcam")]
        public partial KlipperDatabaseMainsailValueWebcam? Webcam { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
