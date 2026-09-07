namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistorySingleJobRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperHistorySingleJobResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperHistorySingleJobRespone);
        #endregion
    }
}
