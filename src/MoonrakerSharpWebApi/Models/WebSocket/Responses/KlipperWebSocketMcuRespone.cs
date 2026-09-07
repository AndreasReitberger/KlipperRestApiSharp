namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketMcuRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("mcu")]
        public partial KlipperStatusMcu? Mcu { get; set; }

        [ObservableProperty]

        [JsonPropertyName("system_stats")]
        public partial KlipperStatusSystemStats? SystemStats { get; set; }

        [ObservableProperty]

        [JsonPropertyName("toolhead")]
        public partial KlipperStatusToolhead? Toolhead { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperWebSocketMcuRespone);
        #endregion
    }
}
