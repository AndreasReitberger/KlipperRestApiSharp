using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketExtruderRespone
    {
        #region Properties
        [JsonProperty("extruder", Required = Required.Always)]
        public KlipperStatusExtruder? Extruder { get; set; }

        [JsonProperty("toolhead", Required = Required.Always)]
        public KlipperStatusToolhead? ToolHead { get; set; }
        // Optional
        [JsonProperty("extruder1")]
        public KlipperStatusExtruder? Extruder1 { get; set; }

        [JsonProperty("extruder2")]
        public KlipperStatusExtruder? Extruder2 { get; set; }

        [JsonProperty("extruder3")]
        public KlipperStatusExtruder? Extruder3 { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
