using System;
using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueWebcamConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("id")]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        [JsonPropertyName("enabled")]
        public partial bool Enabled { get; set; }

        [ObservableProperty]
        [JsonPropertyName("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("service")]
        public partial string Service { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("targetFps")]
        public partial long Fpstarget { get; set; }

        [ObservableProperty]
        [JsonPropertyName("urlStream")]
        public partial string UrlStream { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("urlSnapshot")]
        public partial string UrlSnapshot { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("flipX")]
        public partial bool FlipX { get; set; }

        [ObservableProperty]
        [JsonPropertyName("flipY")]
        public partial bool FlipY { get; set; }

        [ObservableProperty]
        [JsonPropertyName("rotation")]
        public partial int? Rotation { get; set; } = 0;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
