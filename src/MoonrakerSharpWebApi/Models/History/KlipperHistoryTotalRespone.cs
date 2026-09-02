namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryTotalRespone : ObservableObject
    {
        #region Properties

        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperHistoryTotalResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperHistoryTotalRespone);
        #endregion
    }
}
