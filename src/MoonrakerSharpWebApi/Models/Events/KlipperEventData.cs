namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperEventData
    {
        #region Properties
        [JsonPropertyName("data")]
        public object? Data { get; set; }

        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;

        [JsonPropertyName("printer")]
        public string Printer { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
