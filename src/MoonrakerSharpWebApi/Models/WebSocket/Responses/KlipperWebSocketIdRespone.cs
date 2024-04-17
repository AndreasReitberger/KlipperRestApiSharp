using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketIdRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("websocket_id", Required = Required.Always)]
        long websocketId;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
