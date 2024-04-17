using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusPrintStats
    {
        #region Properties
        [JsonProperty("print_duration")]
        public double? PrintDuration { get; set; }

        [JsonProperty("total_duration")]
        public double? TotalDuration { get; set; }

        [JsonProperty("filament_used")]
        public double? FilamentUsed { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; } = string.Empty;

        [JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        public KlipperPrintStates State { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonIgnore]
        public bool ValidPrintState { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
