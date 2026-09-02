namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusSubscriptionRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperPrinterStatusSubscriptionResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
