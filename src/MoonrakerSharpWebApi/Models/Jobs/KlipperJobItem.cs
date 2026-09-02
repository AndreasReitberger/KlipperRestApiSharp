using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobItem : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("end_time")]
        public partial double? EndTime { get; set; }

        [ObservableProperty]
        [JsonPropertyName("filament_used")]
        public partial double FilamentUsed { get; set; }

        [ObservableProperty]
        [JsonPropertyName("filename")]
        public partial string Filename { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("metadata")]
        public partial IGcodeMeta? Metadata { get; set; }

        [ObservableProperty]
        [JsonPropertyName("print_duration")]
        public partial double PrintDuration { get; set; }

        [ObservableProperty]
        [JsonPropertyName("status")]
        //[field: JsonConverter(typeof(StringEnumConverter), true)]
        public partial KlipperJobStates Status { get; set; }


        [ObservableProperty]
        [JsonPropertyName("start_time")]
        public partial double? StartTime { get; set; }

        [ObservableProperty]
        [JsonPropertyName("total_duration")]
        public partial double TotalDuration { get; set; }

        [ObservableProperty]
        [JsonPropertyName("job_id")]
        public partial string JobId { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("exists")]
        public partial bool Exists { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperJobItem);
        #endregion
    }
}
