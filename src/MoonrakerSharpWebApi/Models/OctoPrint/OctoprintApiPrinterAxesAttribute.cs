namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterAxesAttribute : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("inverted")]
        public partial bool Inverted { get; set; }

        [ObservableProperty]

        [JsonPropertyName("speed")]
        public partial long Speed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
