using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketKlipperStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("klippy_connected")]
        bool klippyConnected;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("klippy_state")]
        string klippyState = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("components")]
        List<string> components = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("failed_components")]
        List<object> failedComponents = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("registered_directories")]
        List<string> registeredDirectories = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("warnings")]
        List<object> warnings = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("websocket_count")]
        long websocketCount;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("moonraker_version")]
        string moonrakerVersion = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
