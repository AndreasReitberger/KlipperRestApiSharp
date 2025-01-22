using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("flipH")]
        public partial bool FlipH { get; set; }

        [ObservableProperty]
        
        [JsonProperty("flipV")]
        public partial bool FlipV { get; set; }

        [ObservableProperty]
        
        [JsonProperty("rotate90")]
        public partial bool Rotate90 { get; set; }

        [ObservableProperty]
        
        [JsonProperty("streamUrl")]
        public partial string StreamUrl { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("webcamEnabled")]
        public partial bool WebcamEnabled { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
