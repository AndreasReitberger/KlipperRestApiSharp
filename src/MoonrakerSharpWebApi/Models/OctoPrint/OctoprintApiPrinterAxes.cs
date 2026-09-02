namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterAxes : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("e")]
        public partial OctoprintApiPrinterAxesAttribute? E { get; set; }

        [ObservableProperty]

        [JsonPropertyName("x")]
        public partial OctoprintApiPrinterAxesAttribute? X { get; set; }

        [ObservableProperty]

        [JsonPropertyName("y")]
        public partial OctoprintApiPrinterAxesAttribute? Y { get; set; }

        [ObservableProperty]

        [JsonPropertyName("z")]
        public partial OctoprintApiPrinterAxesAttribute? Z { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
