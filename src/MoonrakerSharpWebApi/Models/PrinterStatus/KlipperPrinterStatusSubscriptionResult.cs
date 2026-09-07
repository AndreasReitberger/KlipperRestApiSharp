namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusSubscriptionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("status")]
        public partial KlipperPrinterStatusSubscriptionStatus? Status { get; set; }

        [ObservableProperty]

        [JsonPropertyName("eventtime")]
        public partial double? Eventtime { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperPrinterStatusSubscriptionResult);
        #endregion
    }
}
