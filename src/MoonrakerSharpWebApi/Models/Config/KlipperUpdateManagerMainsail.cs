using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManagerMainsail
    {
        #region Properties
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("repo")]
        public string Repo { get; set; } = string.Empty;

        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("persistent_files")]
        public object? PersistentFiles { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
