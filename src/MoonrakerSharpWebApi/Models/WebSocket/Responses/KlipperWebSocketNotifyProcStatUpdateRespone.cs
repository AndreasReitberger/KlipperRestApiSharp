using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketNotifyProcStatUpdateRespone : ObservableObject
    {
        #region Properties

        [ObservableProperty]

        [JsonPropertyName("moonraker_stats")]
        public partial MoonrakerStatInfo? MoonrakerStats { get; set; }

        [ObservableProperty]

        [JsonPropertyName("throttled_state")]
        public partial MoonrakerThrottledState? ThrottledState { get; set; }

        [ObservableProperty]

        [JsonPropertyName("cpu_temp")]
        public partial double CpuTemp { get; set; }

        [ObservableProperty]

        [JsonPropertyName("network")]
        public partial Dictionary<string, KlipperNetworkInterface> Network { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("websocket_connections")]
        public partial long WebsocketConnections { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperWebSocketNotifyProcStatUpdateRespone);
        #endregion
    }
}
