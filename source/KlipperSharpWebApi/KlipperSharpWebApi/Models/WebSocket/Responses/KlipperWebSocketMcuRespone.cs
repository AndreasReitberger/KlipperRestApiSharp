using Newtonsoft.Json;

namespace AndreasReitberger.Models.WebSocket
{
    public partial class KlipperWebSocketMcuRespone
    {
        #region Properties
        [JsonProperty("mcu", Required = Required.Always)]
        public KlipperStatusMcu Mcu { get; set; }

        [JsonProperty("system_stats", Required = Required.Always)]
        public KlipperStatusSystemStats SystemStats { get; set; }

        [JsonProperty("toolhead", Required = Required.Always)]
        public KlipperStatusToolhead Toolhead { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
