using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryTotalResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("job_totals")]
        public partial KlipperHistoryJobTotalsResult? JobTotals { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
