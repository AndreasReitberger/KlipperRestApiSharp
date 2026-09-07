namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryTotalResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("job_totals")]
        public partial KlipperHistoryJobTotalsResult? JobTotals { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperHistoryTotalResult);
        #endregion
    }
}
