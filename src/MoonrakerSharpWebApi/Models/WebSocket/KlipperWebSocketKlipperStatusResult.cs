using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketKlipperStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("klippy_connected")]
        bool klippyConnected;

        [ObservableProperty]
        [property: JsonProperty("klippy_state")]
        string klippyState = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("components")]
        List<string> components = [];

        [ObservableProperty]
        [property: JsonProperty("failed_components")]
        List<object> failedComponents = [];

        [ObservableProperty]
        [property: JsonProperty("registered_directories")]
        List<string> registeredDirectories = [];

        [ObservableProperty]
        [property: JsonProperty("warnings")]
        List<object> warnings = [];

        [ObservableProperty]
        [property: JsonProperty("websocket_count")]
        long websocketCount;

        [ObservableProperty]
        [property: JsonProperty("moonraker_version")]
        string moonrakerVersion = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
