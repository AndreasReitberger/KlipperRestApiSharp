using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusPrintStats : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("print_duration")]
        double? printDuration;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("total_duration")]
        double? totalDuration;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filament_used")]
        double? filamentUsed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filename")]
        string filename = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        KlipperPrintStates state;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("message")]
        string message = string.Empty;

        [ObservableProperty, JsonIgnore]
        bool validPrintState = false;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
