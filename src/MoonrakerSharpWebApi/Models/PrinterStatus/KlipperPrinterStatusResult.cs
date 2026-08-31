namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("status")]
        public partial object? Status { get; set; }

        [ObservableProperty]

        [JsonPropertyName("eventtime")]
        public partial double? Eventtime { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
