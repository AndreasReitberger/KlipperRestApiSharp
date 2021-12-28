using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperDirectoryActionResult
    {
        #region Properties
        [JsonProperty("item")]
        public KlipperDirectory Item { get; set; }

        [JsonProperty("source_item")]
        public KlipperDirectory SourceItem { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
