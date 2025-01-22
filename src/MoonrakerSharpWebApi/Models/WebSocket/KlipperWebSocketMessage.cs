using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketMessage : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonProperty("jsonrpc")]
        public partial string Jsonrpc { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("method")]
        public partial string Method { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("params")]
        public partial List<object> Parameters { get; set; } = [];

        [ObservableProperty]
        [JsonProperty("id")]
        public partial long? Id { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
