using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
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

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("fpstarget")]
        public long Fpstarget { get; set; }

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
