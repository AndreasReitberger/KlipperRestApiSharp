using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketMessage : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("jsonrpc")]
        public partial string Jsonrpc { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("method")]
        public partial string Method { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("params")]
        public partial List<object> Parameters { get; set; } = [];

        [ObservableProperty]
        [JsonPropertyName("id")]
        public partial long? Id { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
