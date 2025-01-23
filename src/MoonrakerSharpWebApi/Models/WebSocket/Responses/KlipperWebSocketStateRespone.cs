using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketStateRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("klippy_connected")]
        public partial bool? KlippyConnected { get; set; }

        [ObservableProperty]

        [JsonProperty("klippy_state")]
        public partial string KlippyState { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("components")]
        public partial List<string> Components { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("failed_components")]
        public partial List<object> FailedComponents { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("registered_directories")]
        public partial List<string> RegisteredDirectories { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("warnings")]
        public partial List<string> Warnings { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("websocket_count")]
        public partial long WebsocketCount { get; set; }

        [ObservableProperty]

        [JsonProperty("moonraker_version")]
        public partial string MoonrakerVersion { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
