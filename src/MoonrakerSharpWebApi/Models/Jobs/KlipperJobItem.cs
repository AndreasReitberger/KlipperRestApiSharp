using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobItem : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("end_time")]
        public partial double? EndTime { get; set; }

        [ObservableProperty]
        
        [JsonProperty("filament_used")]
        public partial double FilamentUsed { get; set; }

        [ObservableProperty]
        
        [JsonProperty("filename")]
        public partial string Filename { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("metadata")]
        public partial IGcodeMeta? Metadata { get; set; }

        //public KlipperGcodeMetaResult Metadata;

        [ObservableProperty]
        
        [JsonProperty("print_duration")]
        public partial double PrintDuration { get; set; }

        [ObservableProperty]
        
        [JsonProperty("status")]
        [field: JsonConverter(typeof(StringEnumConverter), true)]
        public partial KlipperJobStates Status { get; set; }

        //public string Status;

        [ObservableProperty]
        
        [JsonProperty("start_time")]
        public partial double? StartTime { get; set; }

        [ObservableProperty]
        
        [JsonProperty("total_duration")]
        public partial double TotalDuration { get; set; }

        [ObservableProperty]
        
        [JsonProperty("job_id")]
        public partial string JobId { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("exists")]
        public partial bool Exists { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
