using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusIdleTimeout : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        KlipperIdleStates state;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printing_time")]
        double? printingTime;

        [ObservableProperty, JsonIgnore]
        bool validState = false;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
