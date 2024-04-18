using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobItem : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("end_time")]
        double? endTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filament_used")]
        double filamentUsed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filename")]
        string filename = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("metadata")]
        IGcodeMeta? metadata;
        //public KlipperGcodeMetaResult Metadata;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("print_duration")]
        double printDuration;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        KlipperJobStates status;
        //public string Status;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("start_time")]
        double? startTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("total_duration")]
        double totalDuration;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("job_id")]
        string jobId = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("exists")]
        bool exists;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
