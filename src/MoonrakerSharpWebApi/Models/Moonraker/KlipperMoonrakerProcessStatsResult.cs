using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMoonrakerProcessStatsResult
    {
        #region Properties
        [JsonProperty("moonraker_stats")]
        public List<MoonrakerStatInfo> MoonrakerStats { get; set; } = [];

        [JsonProperty("throttled_state")]
        public MoonrakerThrottledState? ThrottledState { get; set; }

        [JsonProperty("cpu_temp")]
        public double CpuTemp { get; set; }

        [JsonProperty("network")]
        public Dictionary<string, KlipperNetworkInterface> Network { get; set; } = [];

        [JsonProperty("websocket_connections")]
        public long WebsocketConnections { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
