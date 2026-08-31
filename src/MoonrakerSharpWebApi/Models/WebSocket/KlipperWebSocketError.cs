using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketError : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("code")]
        public partial long Code { get; set; }

        [ObservableProperty]
        [JsonPropertyName("message")]
        public partial string Message { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
