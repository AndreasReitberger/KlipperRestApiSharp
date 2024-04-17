using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketExtruderRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("extruder", Required = Required.Always)]
        KlipperStatusExtruder? extruder;

        [ObservableProperty]
        [property: JsonProperty("toolhead", Required = Required.Always)]
        KlipperStatusToolhead? toolHead;

        [ObservableProperty]
        [property: JsonProperty("extruder1")]
        KlipperStatusExtruder? extruder1;

        [ObservableProperty]
        [property: JsonProperty("extruder2")]
        KlipperStatusExtruder? extruder2;

        [ObservableProperty]
        [property: JsonProperty("extruder3")]
        KlipperStatusExtruder? extruder3;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
