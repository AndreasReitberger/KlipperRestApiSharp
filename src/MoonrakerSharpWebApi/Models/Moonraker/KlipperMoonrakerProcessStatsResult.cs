using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMoonrakerProcessStatsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("moonraker_stats")]
        List<MoonrakerStatInfo> moonrakerStats = [];

        [ObservableProperty]
        [property: JsonProperty("throttled_state")]
        MoonrakerThrottledState? throttledState;

        [ObservableProperty]
        [property: JsonProperty("cpu_temp")]
        double cpuTemp;

        [ObservableProperty]
        [property: JsonProperty("network")]
        Dictionary<string, KlipperNetworkInterface> network = [];

        [ObservableProperty]
        [property: JsonProperty("websocket_connections")]
        long websocketConnections;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
