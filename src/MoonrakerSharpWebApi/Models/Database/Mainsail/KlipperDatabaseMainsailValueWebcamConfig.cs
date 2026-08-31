using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueWebcamConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("icon")]
        public partial string Icon { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("service")]
        public partial string Service { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("targetFps")]
        public partial long TargetFps { get; set; }

        [ObservableProperty]
        [JsonPropertyName("url")]
        public partial string Url { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("flipX")]
        public partial bool FlipX { get; set; }

        [ObservableProperty]
        [JsonPropertyName("flipY")]
        public partial bool FlipY { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
