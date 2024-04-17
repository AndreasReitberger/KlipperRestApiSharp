using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiVersionResult
    {
        #region Properties
        [JsonProperty("server")]
        public string Server { get; set; } = string.Empty;

        [JsonProperty("api")]
        public string Api { get; set; } = string.Empty;

        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
