using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryJobTotalsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("total_jobs")]
        long totalJobs;

        [ObservableProperty]
        [property: JsonProperty("total_time")]
        double totalTime;

        [ObservableProperty]
        [property: JsonProperty("total_print_time")]
        double totalPrintTime;

        [ObservableProperty]
        [property: JsonProperty("total_filament_used")]
        double totalFilamentUsed;

        [ObservableProperty]
        [property: JsonProperty("longest_job")]
        double longestJob;

        [ObservableProperty]
        [property: JsonProperty("longest_print")]
        double longestPrint;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
