using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketMessage : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("jsonrpc")]
        string jsonrpc = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("method")]
        string method = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("params")]
        List<object> parameters = [];

        [ObservableProperty]
        [property: JsonProperty("id")]
        long? id;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
