using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperEndstopQueryResult
    {
        #region Properties
        [JsonProperty("y")]
        public string Y { get; set; } = string.Empty;

        [JsonProperty("x")]
        public string X { get; set; } = string.Empty;

        [JsonProperty("z")]
        public string Z { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
