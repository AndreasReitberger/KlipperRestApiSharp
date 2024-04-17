using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiServerStatusResult
    {
        #region Properties
        [JsonProperty("server")]
        public string Server { get; set; } = string.Empty;

        [JsonProperty("safemode")]
        public object? Safemode { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
