namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperRootInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("permissions")]
        public partial string Permissions { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperRootInfo);
        #endregion
    }
}
