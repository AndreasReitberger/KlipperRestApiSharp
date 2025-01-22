using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketError : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonProperty("code")]
        public partial long Code { get; set; }

        [ObservableProperty]
        [JsonProperty("message")]
        public partial string Message { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
