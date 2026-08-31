namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("temperature")]
        public partial OctoprintApiPrinterStateTemperature? Temperature { get; set; }

        [ObservableProperty]

        [JsonPropertyName("sd")]
        public partial OctoprintApiPrinterStateSd? Sd { get; set; }

        [ObservableProperty]

        [JsonPropertyName("state")]
        public partial OctoprintApiPrinterState? State { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
