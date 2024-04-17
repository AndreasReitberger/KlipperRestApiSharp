using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterVolume
    {
        #region Properties
        [JsonProperty("depth")]
        public long Depth { get; set; }

        [JsonProperty("formFactor")]
        public string FormFactor { get; set; } = string.Empty;

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; } = string.Empty;

        [JsonProperty("width")]
        public long Width { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
