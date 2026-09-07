namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigVirtualSdcard : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("path")]
        public partial string Path { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperConfigVirtualSdcard);
        #endregion
    }
}
