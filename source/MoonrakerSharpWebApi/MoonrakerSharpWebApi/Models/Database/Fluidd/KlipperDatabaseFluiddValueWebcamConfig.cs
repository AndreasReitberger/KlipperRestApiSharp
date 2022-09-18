using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueWebcamConfig
    {
        #region Properties
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("targetFps")]
        public long Fpstarget { get; set; }

        [JsonProperty("urlStream")]
        public string UrlStream { get; set; }

        [JsonProperty("urlSnapshot")]
        public string UrlSnapshot { get; set; }

        [JsonProperty("flipX")]
        public bool FlipX { get; set; }

        [JsonProperty("flipY")]
        public bool FlipY { get; set; }

        [JsonProperty("rotation")]
        public int? Rotation { get; set; } = 0;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
