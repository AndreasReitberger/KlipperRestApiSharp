using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperDatabaseMainsailValueWebcamConfig
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("targetFps")]
        public long TargetFps { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("flipX")]
        public bool FlipX { get; set; }

        [JsonProperty("flipY")]
        public bool FlipY { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
