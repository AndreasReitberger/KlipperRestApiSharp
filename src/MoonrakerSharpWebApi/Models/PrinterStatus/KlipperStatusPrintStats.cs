using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusPrintStats : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("print_duration")]
        double? printDuration;

        [ObservableProperty]
        [property: JsonProperty("total_duration")]
        double? totalDuration;

        [ObservableProperty]
        [property: JsonProperty("filament_used")]
        double? filamentUsed;

        [ObservableProperty]
        [property: JsonProperty("filename")]
        string filename = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        KlipperPrintStates state;

        [ObservableProperty]
        [property: JsonProperty("message")]
        string message = string.Empty;

        [JsonIgnore]
        [ObservableProperty]
        bool validPrintState = false;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
