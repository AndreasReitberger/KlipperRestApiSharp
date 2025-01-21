using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonProperty("jsonrpc")]
        public partial string Jsonrpc { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("result")]
        public partial object? Result { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("id")]
        public partial long Id { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
