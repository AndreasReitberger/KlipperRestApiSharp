using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDistribution
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("version_parts")]
        public KlipperVersion? KlipperVersion { get; set; }

        [JsonProperty("like")]
        public string Like { get; set; } = string.Empty;

        [JsonProperty("codename")]
        public string Codename { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
