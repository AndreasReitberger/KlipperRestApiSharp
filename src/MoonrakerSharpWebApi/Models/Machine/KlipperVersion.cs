using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperVersion
    {
        #region Properties
        [JsonProperty("major")]
        public long Major { get; set; }

        [JsonProperty("minor")]
        public string Minor { get; set; } = string.Empty;

        [JsonProperty("build_number")]
        public string BuildNumber { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
