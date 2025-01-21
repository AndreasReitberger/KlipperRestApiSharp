using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryJobTotalsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("total_jobs")]
        public partial long TotalJobs { get; set; }

        [ObservableProperty]
        
        [JsonProperty("total_time")]
        public partial double TotalTime { get; set; }

        [ObservableProperty]
        
        [JsonProperty("total_print_time")]
        public partial double TotalPrintTime { get; set; }

        [ObservableProperty]
        
        [JsonProperty("total_filament_used")]
        public partial double TotalFilamentUsed { get; set; }

        [ObservableProperty]
        
        [JsonProperty("longest_job")]
        public partial double LongestJob { get; set; }

        [ObservableProperty]
        
        [JsonProperty("longest_print")]
        public partial double LongestPrint { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
