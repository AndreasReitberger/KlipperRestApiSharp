namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiVersionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("server")]
        public partial string Server { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("api")]
        public partial string Api { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("text")]
        public partial string Text { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
