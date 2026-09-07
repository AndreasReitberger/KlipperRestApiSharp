namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperEndstopQueryResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("y")]
        public partial string Y { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("x")]
        public partial string X { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("z")]
        public partial string Z { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperEndstopQueryResult);
        #endregion
    }
}
