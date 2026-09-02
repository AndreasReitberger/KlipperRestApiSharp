using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("jsonrpc")]
        public partial string Jsonrpc { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("result")]
        public partial object? Result { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("id")]
        public partial long Id { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
