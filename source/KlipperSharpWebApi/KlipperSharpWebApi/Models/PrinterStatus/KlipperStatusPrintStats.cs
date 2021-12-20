using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusPrintStats
    {
        #region Properties
        [JsonProperty("print_duration")]
        public long PrintDuration { get; set; }

        [JsonProperty("total_duration")]
        public long TotalDuration { get; set; }

        [JsonProperty("filament_used")]
        public long FilamentUsed { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
