namespace AndreasReitberger.API.Moonraker.Models
{
    // Maybe delete later?
    public partial class KlipperPrinterStatusQueryRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperPrinterStatusQueryResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
