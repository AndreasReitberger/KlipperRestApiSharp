namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("bed")]
        public partial OctoprintApiPrinterStateTemperatureInfo? Bed { get; set; }

        [ObservableProperty]

        [JsonPropertyName("time")]
        public partial long? Time { get; set; }

        [ObservableProperty]

        [JsonPropertyName("tool0")]
        public partial OctoprintApiPrinterStateTemperatureInfo? Tool0 { get; set; }

        [ObservableProperty]

        [JsonPropertyName("tool1")]
        public partial OctoprintApiPrinterStateTemperatureInfo? Tool1 { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
