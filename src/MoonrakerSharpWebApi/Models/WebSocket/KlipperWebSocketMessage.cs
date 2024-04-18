using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketMessage : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("jsonrpc")]
        string jsonrpc = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("method")]
        string method = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("params")]
        List<object> parameters = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("id")]
        long? id;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
