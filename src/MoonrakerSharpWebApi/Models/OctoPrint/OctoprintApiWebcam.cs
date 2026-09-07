namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("flipH")]
        public partial bool FlipH { get; set; }

        [ObservableProperty]

        [JsonPropertyName("flipV")]
        public partial bool FlipV { get; set; }

        [ObservableProperty]

        [JsonPropertyName("rotate90")]
        public partial bool Rotate90 { get; set; }

        [ObservableProperty]

        [JsonPropertyName("streamUrl")]
        public partial string StreamUrl { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("webcamEnabled")]
        public partial bool WebcamEnabled { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.OctoprintApiWebcam);
        #endregion
    }
}
