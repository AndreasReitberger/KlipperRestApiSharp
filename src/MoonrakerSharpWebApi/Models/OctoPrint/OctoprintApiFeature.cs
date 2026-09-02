namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiFeature : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("sdSupport")]
        public partial bool SdSupport { get; set; }

        [ObservableProperty]

        [JsonPropertyName("temperatureGraph")]
        public partial bool TemperatureGraph { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
