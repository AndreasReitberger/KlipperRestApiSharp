namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperPrinterStatusResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperPrinterStatusRespone);
        #endregion
    }
}
