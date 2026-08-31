using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMoonrakerProcessStatsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("moonraker_stats")]
        public partial List<MoonrakerStatInfo> MoonrakerStats { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("throttled_state")]
        public partial MoonrakerThrottledState? ThrottledState { get; set; }

        [ObservableProperty]

        [JsonPropertyName("cpu_temp")]
        public partial double? CpuTemp { get; set; }

        [ObservableProperty]

        [JsonPropertyName("network")]
        public partial Dictionary<string, KlipperNetworkInterface> Network { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("websocket_connections")]
        public partial long WebsocketConnections { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
