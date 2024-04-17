using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketStateRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("klippy_connected", Required = Required.Always)]
        bool klippyConnected;

        [ObservableProperty]
        [property: JsonProperty("klippy_state", Required = Required.Always)]
        string klippyState = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("components", Required = Required.Always)]
        List<string> components = [];

        [ObservableProperty]
        [property: JsonProperty("failed_components", Required = Required.Always)]
        List<object> failedComponents = [];

        [ObservableProperty]
        [property: JsonProperty("registered_directories", Required = Required.Always)]
        List<string> registeredDirectories = [];

        [ObservableProperty]
        [property: JsonProperty("warnings", Required = Required.Always)]
        List<string> warnings = [];

        [ObservableProperty]
        [property: JsonProperty("websocket_count", Required = Required.Always)]
        long websocketCount;

        [ObservableProperty]
        [property: JsonProperty("moonraker_version", Required = Required.Always)]
        string moonrakerVersion = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
