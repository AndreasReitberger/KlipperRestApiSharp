using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryJobTotalsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("total_jobs")]
        long totalJobs;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("total_time")]
        double totalTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("total_print_time")]
        double totalPrintTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("total_filament_used")]
        double totalFilamentUsed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("longest_job")]
        double longestJob;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("longest_print")]
        double longestPrint;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
