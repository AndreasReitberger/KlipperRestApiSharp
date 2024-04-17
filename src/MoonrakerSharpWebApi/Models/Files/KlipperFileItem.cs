using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFileItem
    {
        #region Properties
        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("root")]
        public string Root { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
