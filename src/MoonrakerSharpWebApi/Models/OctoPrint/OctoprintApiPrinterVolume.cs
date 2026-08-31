namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterVolume : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("depth")]
        public partial long Depth { get; set; }

        [ObservableProperty]

        [JsonPropertyName("formFactor")]
        public partial string FormFactor { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("height")]
        public partial long Height { get; set; }

        [ObservableProperty]

        [JsonPropertyName("origin")]
        public partial string Origin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("width")]
        public partial long Width { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
