namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperVersion : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("major")]
        public partial long Major { get; set; }

        [ObservableProperty]

        [JsonPropertyName("minor")]
        public partial string Minor { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("build_number")]
        public partial string BuildNumber { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperVersion);
        #endregion
    }
}
