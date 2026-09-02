using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketStateRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("klippy_connected")]
        public partial bool? KlippyConnected { get; set; }

        [ObservableProperty]

        [JsonPropertyName("klippy_state")]
        public partial string KlippyState { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("components")]
        public partial List<string> Components { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("failed_components")]
        public partial List<object> FailedComponents { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("registered_directories")]
        public partial List<string> RegisteredDirectories { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("warnings")]
        public partial List<string> Warnings { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("websocket_count")]
        public partial long WebsocketCount { get; set; }

        [ObservableProperty]

        [JsonPropertyName("moonraker_version")]
        public partial string MoonrakerVersion { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
