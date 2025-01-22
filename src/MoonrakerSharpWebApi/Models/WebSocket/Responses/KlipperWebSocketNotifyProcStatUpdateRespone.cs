using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketNotifyProcStatUpdateRespone : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        
        [JsonProperty("moonraker_stats")]
        public partial MoonrakerStatInfo? MoonrakerStats { get; set; }

        [ObservableProperty]
        
        [JsonProperty("throttled_state")]
        public partial MoonrakerThrottledState? ThrottledState { get; set; }

        [ObservableProperty]
        
        [JsonProperty("cpu_temp")]
        public partial double CpuTemp { get; set; }

        [ObservableProperty]
        
        [JsonProperty("network")]
        public partial Dictionary<string, KlipperNetworkInterface> Network { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("websocket_connections")]
        public partial long WebsocketConnections { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
