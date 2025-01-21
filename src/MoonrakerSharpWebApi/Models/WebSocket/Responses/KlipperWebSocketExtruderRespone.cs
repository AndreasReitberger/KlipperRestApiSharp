using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketExtruderRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonProperty("extruder")]
        public partial KlipperStatusExtruder? Extruder { get; set; }

        [ObservableProperty]
        [JsonProperty("toolhead")]
        public partial KlipperStatusToolhead? ToolHead { get; set; }

        [ObservableProperty]
        [JsonProperty("extruder1")]
        public partial KlipperStatusExtruder? Extruder1 { get; set; }

        [ObservableProperty]
        [JsonProperty("extruder2")]
        public partial KlipperStatusExtruder? Extruder2 { get; set; }

        [ObservableProperty]
        [JsonProperty("extruder3")]
        public partial KlipperStatusExtruder? Extruder3 { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
