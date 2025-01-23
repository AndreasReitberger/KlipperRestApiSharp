using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketIdRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("websocket_id")]
        public partial long WebsocketId { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
