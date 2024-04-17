using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryJobTotalsResult
    {
        #region Properties
        [JsonProperty("total_jobs")]
        public long TotalJobs { get; set; }

        [JsonProperty("total_time")]
        public double TotalTime { get; set; }

        [JsonProperty("total_print_time")]
        public double TotalPrintTime { get; set; }

        [JsonProperty("total_filament_used")]
        public double TotalFilamentUsed { get; set; }

        [JsonProperty("longest_job")]
        public double LongestJob { get; set; }

        [JsonProperty("longest_print")]
        public double LongestPrint { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
