using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueWebcamConfig
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("icon")]
        public string Icon { get; set; } = string.Empty;

        [JsonProperty("service")]
        public string Service { get; set; } = string.Empty;

        [JsonProperty("targetFps")]
        public long TargetFps { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty("flipX")]
        public bool FlipX { get; set; }

        [JsonProperty("flipY")]
        public bool FlipY { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
