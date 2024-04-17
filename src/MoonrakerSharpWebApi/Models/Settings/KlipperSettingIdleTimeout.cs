using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingIdleTimeout
    {
        #region Properties
        [JsonProperty("gcode")]
        public string Gcode { get; set; } = string.Empty;

        [JsonProperty("timeout")]
        public long Timeout { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
