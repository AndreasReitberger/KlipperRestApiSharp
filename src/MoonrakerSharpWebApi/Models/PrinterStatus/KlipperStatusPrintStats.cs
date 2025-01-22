using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusPrintStats : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("print_duration")]
        public partial double? PrintDuration { get; set; }

        [ObservableProperty]
        
        [JsonProperty("total_duration")]
        public partial double? TotalDuration { get; set; }

        [ObservableProperty]
        
        [JsonProperty("filament_used")]
        public partial double? FilamentUsed { get; set; }

        [ObservableProperty]
        
        [JsonProperty("filename")]
        public partial string Filename { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("state")]
        [field: JsonConverter(typeof(StringEnumConverter), true)]
        public partial KlipperPrintStates State { get; set; }

        [ObservableProperty]
        
        [JsonProperty("message")]
        public partial string Message { get; set; } = string.Empty;

        [ObservableProperty]
        
        public partial bool ValidPrintState { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
