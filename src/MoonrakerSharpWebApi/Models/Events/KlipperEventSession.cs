namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperEventSession
    {
        #region Properties
        [JsonPropertyName("callback_id")]
        public long CallbackId { get; set; }

        [JsonPropertyName("data")]
        public object? Data { get; set; }

        [JsonPropertyName("session")]
        public string Session { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperEventSession);
        #endregion
    }
}
