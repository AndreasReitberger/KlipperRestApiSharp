using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketStateRespone
    {
        #region Properties
        [JsonProperty("klippy_connected", Required = Required.Always)]
        public bool KlippyConnected { get; set; }

        [JsonProperty("klippy_state", Required = Required.Always)]
        public string KlippyState { get; set; } = string.Empty;

        [JsonProperty("components", Required = Required.Always)]
        public List<string> Components { get; set; } = [];

        [JsonProperty("failed_components", Required = Required.Always)]
        public List<object> FailedComponents { get; set; } = [];

        [JsonProperty("registered_directories", Required = Required.Always)]
        public List<string> RegisteredDirectories { get; set; } = [];

        [JsonProperty("warnings", Required = Required.Always)]
        public List<string> Warnings { get; set; } = [];

        [JsonProperty("websocket_count", Required = Required.Always)]
        public long WebsocketCount { get; set; }

        [JsonProperty("moonraker_version", Required = Required.Always)]
        public string MoonrakerVersion { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
