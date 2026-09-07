namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperVirtualization : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("virt_type")]
        public partial string VirtType { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("virt_identifier")]
        public partial string VirtIdentifier { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperVirtualization);
        #endregion
    }
}
