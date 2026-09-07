namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFileItem : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("path")]
        public partial string Path { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("root")]
        public partial string Root { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperFileItem);
        #endregion
    }
}
