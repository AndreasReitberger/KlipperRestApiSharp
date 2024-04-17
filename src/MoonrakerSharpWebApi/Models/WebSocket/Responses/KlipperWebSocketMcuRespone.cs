using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketMcuRespone
    {
        #region Properties
        [JsonProperty("mcu", Required = Required.Always)]
        public KlipperStatusMcu? Mcu { get; set; }

        [JsonProperty("system_stats", Required = Required.Always)]
        public KlipperStatusSystemStats? SystemStats { get; set; }

        [JsonProperty("toolhead", Required = Required.Always)]
        public KlipperStatusToolhead? Toolhead { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
