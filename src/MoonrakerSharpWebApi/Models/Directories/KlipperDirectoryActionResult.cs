namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryActionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("item")]
        public partial KlipperDirectory? Item { get; set; }

        [ObservableProperty]

        [JsonPropertyName("source_item")]
        public partial KlipperDirectory? SourceItem { get; set; }

        [ObservableProperty]

        [JsonPropertyName("action")]
        public partial string Action { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
