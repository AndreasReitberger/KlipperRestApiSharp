using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintAbiJobInfoFile
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("origin")]
        public string Origin { get; set; } = string.Empty;

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public long Size { get; set; }

        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public long Date { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
