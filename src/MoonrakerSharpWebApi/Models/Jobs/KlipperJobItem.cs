using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobItem
    {
        #region Properties
        [JsonProperty("end_time")]
        public double? EndTime { get; set; }

        [JsonProperty("filament_used")]
        public double FilamentUsed { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; } = string.Empty;

        [JsonProperty("metadata")]
        public IGcodeMeta? Metadata { get; set; }
        //public KlipperGcodeMetaResult Metadata { get; set; }

        [JsonProperty("print_duration")]
        public double PrintDuration { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        public KlipperJobStates Status { get; set; }
        //public string Status { get; set; }

        [JsonProperty("start_time")]
        public double? StartTime { get; set; }

        [JsonProperty("total_duration")]
        public double TotalDuration { get; set; }

        [JsonProperty("job_id")]
        public string JobId { get; set; } = string.Empty;

        [JsonProperty("exists")]
        public bool Exists { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
