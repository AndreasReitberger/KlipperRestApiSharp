namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperHistoryResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperHistoryRespone);
        #endregion
    }
}
