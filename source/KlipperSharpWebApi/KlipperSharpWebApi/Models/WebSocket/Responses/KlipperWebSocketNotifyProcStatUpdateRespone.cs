using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models.WebSocket
{
    public partial class KlipperWebSocketNotifyProcStatUpdateRespone
    {
        #region Properties

        [JsonProperty("moonraker_stats", Required = Required.Always)]
        public MoonrakerStatInfo MoonrakerStats { get; set; }

        [JsonProperty("throttled_state")]
        public MoonrakerThrottledState ThrottledState { get; set; }

        [JsonProperty("cpu_temp", Required = Required.Always)]
        public double CpuTemp { get; set; }

        [JsonProperty("network", Required = Required.Always)]
        public Dictionary<string, KlipperNetworkInterface> Network { get; set; }

        [JsonProperty("websocket_connections", Required = Required.Always)]
        public long WebsocketConnections { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
