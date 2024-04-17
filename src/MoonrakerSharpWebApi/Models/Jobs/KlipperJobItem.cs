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
        [property: JsonProperty("end_time")]
        double? endTime;

        [ObservableProperty]
        [property: JsonProperty("filament_used")]
        double filamentUsed;

        [ObservableProperty]
        [property: JsonProperty("filename")]
        string filename = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("metadata")]
        IGcodeMeta? metadata;
        //public KlipperGcodeMetaResult Metadata;

        [ObservableProperty]
        [property: JsonProperty("print_duration")]
        double printDuration;

        [ObservableProperty]
        [property: JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        KlipperJobStates status;
        //public string Status;

        [ObservableProperty]
        [property: JsonProperty("start_time")]
        double? startTime;

        [ObservableProperty]
        [property: JsonProperty("total_duration")]
        double totalDuration;

        [ObservableProperty]
        [property: JsonProperty("job_id")]
        string jobId = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("exists")]
        bool exists;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
