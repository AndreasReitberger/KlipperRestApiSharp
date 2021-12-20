using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperHistoryTotalResult
    {
        #region Properties
        [JsonProperty("job_totals")]
        public KlipperHistoryJobTotalsResult JobTotals { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
