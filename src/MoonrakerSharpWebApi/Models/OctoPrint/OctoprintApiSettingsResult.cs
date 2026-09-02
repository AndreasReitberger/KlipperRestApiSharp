using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiSettingsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("plugins")]
        public partial Dictionary<string, OctoprintApiPlugin> Plugins { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("feature")]
        public partial OctoprintApiFeature? Feature { get; set; }

        [ObservableProperty]

        [JsonPropertyName("webcam")]
        public partial OctoprintApiWebcam? Webcam { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
