using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketExtruderRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder")]
        KlipperStatusExtruder? extruder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("toolhead")]
        KlipperStatusToolhead? toolHead;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder1")]
        KlipperStatusExtruder? extruder1;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder2")]
        KlipperStatusExtruder? extruder2;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder3")]
        KlipperStatusExtruder? extruder3;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
