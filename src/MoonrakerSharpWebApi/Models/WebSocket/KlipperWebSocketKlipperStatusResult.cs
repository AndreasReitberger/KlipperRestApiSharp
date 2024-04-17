using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketKlipperStatusResult
    {
        #region Properties
        [JsonProperty("klippy_connected")]
        public bool KlippyConnected { get; set; }

        [JsonProperty("klippy_state")]
        public string KlippyState { get; set; } = string.Empty;

        [JsonProperty("components")]
        public List<string> Components { get; set; } = [];

        [JsonProperty("failed_components")]
        public List<object> FailedComponents { get; set; } = [];

        [JsonProperty("registered_directories")]
        public List<string> RegisteredDirectories { get; set; } = [];

        [JsonProperty("warnings")]
        public List<object> Warnings { get; set; } = [];

        [JsonProperty("websocket_count")]
        public long WebsocketCount { get; set; }

        [JsonProperty("moonraker_version")]
        public string MoonrakerVersion { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
