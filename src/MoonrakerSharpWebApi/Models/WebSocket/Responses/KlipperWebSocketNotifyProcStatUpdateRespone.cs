using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketNotifyProcStatUpdateRespone : ObservableObject
    {
        #region Properties

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("moonraker_stats")]
        MoonrakerStatInfo? moonrakerStats;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("throttled_state")]
        MoonrakerThrottledState? throttledState;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cpu_temp")]
        double cpuTemp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("network")]
        Dictionary<string, KlipperNetworkInterface> network = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("websocket_connections")]
        long websocketConnections;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
