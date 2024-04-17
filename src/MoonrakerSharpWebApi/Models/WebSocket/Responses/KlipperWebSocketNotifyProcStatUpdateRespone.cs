using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketNotifyProcStatUpdateRespone : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        [property: JsonProperty("moonraker_stats", Required = Required.Always)]
        MoonrakerStatInfo? moonrakerStats;

        [ObservableProperty]
        [property: JsonProperty("throttled_state")]
        MoonrakerThrottledState? throttledState;

        [ObservableProperty]
        [property: JsonProperty("cpu_temp", Required = Required.Always)]
        double cpuTemp;

        [ObservableProperty]
        [property: JsonProperty("network", Required = Required.Always)]
        Dictionary<string, KlipperNetworkInterface> network = [];

        [ObservableProperty]
        [property: JsonProperty("websocket_connections", Required = Required.Always)]
        long websocketConnections;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
