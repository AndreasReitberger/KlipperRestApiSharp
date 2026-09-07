namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDiskUsage : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("total")]
        public partial long Total { get; set; }

        [ObservableProperty]

        [JsonPropertyName("used")]
        public partial long Used { get; set; }

        [ObservableProperty]

        [JsonPropertyName("free")]
        public partial long Free { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDiskUsage);
        #endregion
    }
}
