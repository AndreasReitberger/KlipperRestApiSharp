using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcode
    {
        #region Properties
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("time")]
        public double Time { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
