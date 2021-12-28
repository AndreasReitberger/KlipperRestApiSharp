using AndreasReitberger.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusPrintStats
    {
        #region Properties
        [JsonProperty("print_duration")]
        public double PrintDuration { get; set; }

        [JsonProperty("total_duration")]
        public double TotalDuration { get; set; }

        [JsonProperty("filament_used")]
        public double FilamentUsed { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        public KlipperPrintStates State { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonIgnore]
        public bool ValidPrintState { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
