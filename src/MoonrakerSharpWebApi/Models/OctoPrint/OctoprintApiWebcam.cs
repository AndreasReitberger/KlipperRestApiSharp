using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiWebcam
    {
        #region Properties
        [JsonProperty("flipH")]
        public bool FlipH { get; set; }

        [JsonProperty("flipV")]
        public bool FlipV { get; set; }

        [JsonProperty("rotate90")]
        public bool Rotate90 { get; set; }

        [JsonProperty("streamUrl")]
        public string StreamUrl { get; set; } = string.Empty;

        [JsonProperty("webcamEnabled")]
        public bool WebcamEnabled { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
