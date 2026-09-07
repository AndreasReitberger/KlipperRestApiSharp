namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class MoonrakerThrottledState : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("rx_bytes")]
        public partial long RxBytes { get; set; }

        [ObservableProperty]

        [JsonPropertyName("tx_bytes")]
        public partial long TxBytes { get; set; }

        [ObservableProperty]

        [JsonPropertyName("bandwidth")]
        public partial double Bandwidth { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.MoonrakerThrottledState);
        #endregion
    }
}
