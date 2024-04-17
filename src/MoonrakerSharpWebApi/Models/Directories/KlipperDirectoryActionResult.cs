using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryActionResult
    {
        #region Properties
        [JsonProperty("item")]
        public KlipperDirectory? Item { get; set; }

        [JsonProperty("source_item")]
        public KlipperDirectory? SourceItem { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
