namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryJobTotalsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("total_jobs")]
        public partial long TotalJobs { get; set; }

        [ObservableProperty]

        [JsonPropertyName("total_time")]
        public partial double TotalTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("total_print_time")]
        public partial double TotalPrintTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("total_filament_used")]
        public partial double TotalFilamentUsed { get; set; }

        [ObservableProperty]

        [JsonPropertyName("longest_job")]
        public partial double LongestJob { get; set; }

        [ObservableProperty]

        [JsonPropertyName("longest_print")]
        public partial double LongestPrint { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
