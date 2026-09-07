using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValue : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("dashboard")]
        public partial KlipperDatabaseMainsailValueDashboard? Dashboard { get; set; }

        [ObservableProperty]
        [JsonPropertyName("general")]
        public partial KlipperDatabaseMainsailValueGeneral? General { get; set; }

        [ObservableProperty]
        [JsonPropertyName("heightmap")]
        public partial KlipperDatabaseMainsailValueHeightmapSettings? Heightmap { get; set; }

        [ObservableProperty]
        [JsonPropertyName("init")]
        public partial bool Init { get; set; }

        [ObservableProperty]
        [JsonPropertyName("presets")]
        public partial List<KlipperDatabaseMainsailValuePreset> Presets { get; set; } = [];

        [ObservableProperty]
        [JsonPropertyName("remote_printers")]
        public partial List<object> PemotePrinters { get; set; } = [];

        [ObservableProperty]
        [JsonPropertyName("settings")]
        public partial KlipperDatabaseMainsailValueSettings? Settings { get; set; }

        [ObservableProperty]
        [JsonPropertyName("webcam")]
        public partial KlipperDatabaseMainsailValueWebcam? Webcam { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseMainsailValue);
        #endregion
    }
}
