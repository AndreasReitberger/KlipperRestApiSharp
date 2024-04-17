using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryTotalResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("job_totals")]
        KlipperHistoryJobTotalsResult? jobTotals;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
