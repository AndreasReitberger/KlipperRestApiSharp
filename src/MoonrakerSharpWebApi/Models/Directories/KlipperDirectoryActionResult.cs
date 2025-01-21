using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryActionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("item")]
        public partial KlipperDirectory? Item { get; set; }

        [ObservableProperty]
        
        [JsonProperty("source_item")]
        public partial KlipperDirectory? SourceItem { get; set; }

        [ObservableProperty]
        
        [JsonProperty("action")]
        public partial string Action { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
