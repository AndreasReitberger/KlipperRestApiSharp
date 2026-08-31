namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateSd : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("ready")]
        public partial bool? Ready { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
