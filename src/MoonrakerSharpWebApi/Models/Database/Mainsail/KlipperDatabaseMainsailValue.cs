using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValue
    {
        #region Properties
        [JsonProperty("dashboard")]
        public KlipperDatabaseMainsailValueDashboard? Dashboard { get; set; }

        [JsonProperty("general")]
        public KlipperDatabaseMainsailValueGeneral? General { get; set; }

        [JsonProperty("heightmap")]
        public KlipperDatabaseMainsailValueHeightmapSettings? Heightmap { get; set; }

        [JsonProperty("init")]
        public bool Init { get; set; }

        [JsonProperty("presets")]
        public List<KlipperDatabaseMainsailValuePreset> Presets { get; set; } = [];

        [JsonProperty("remote_printers")]
        public List<object> RemotePrinters { get; set; } = [];

        [JsonProperty("settings")]
        public KlipperDatabaseMainsailValueSettings? Settings { get; set; }

        [JsonProperty("webcam")]
        public KlipperDatabaseMainsailValueWebcam? Webcam { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
