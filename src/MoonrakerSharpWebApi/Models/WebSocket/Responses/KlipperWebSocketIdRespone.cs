namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketIdRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("websocket_id")]
        public partial long WebsocketId { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
