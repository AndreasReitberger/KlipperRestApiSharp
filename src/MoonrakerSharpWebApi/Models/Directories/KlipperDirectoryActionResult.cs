using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryActionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("item")]
        public KlipperDirectory? item;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("source_item")]
        public KlipperDirectory? sourceItem;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("action")]
        public string action = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
