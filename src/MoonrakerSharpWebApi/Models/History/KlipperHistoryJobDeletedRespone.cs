namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryJobDeletedRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperHistoryJobDeletedResult? Result { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperHistoryJobDeletedRespone);
        #endregion
    }
}
