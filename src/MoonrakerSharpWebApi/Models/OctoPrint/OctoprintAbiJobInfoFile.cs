namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintAbiJobInfoFile : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("origin")]
        public partial string Origin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("size")]
        public partial long Size { get; set; }

        [ObservableProperty]

        [JsonPropertyName("date")]
        public partial long Date { get; set; }

        [ObservableProperty]

        [JsonPropertyName("path")]
        public partial string Path { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
