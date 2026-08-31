namespace AndreasReitberger.API.Moonraker.Models
{
    // Maybe delete later?
    public partial class KlipperPrinterStatusQueryRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperPrinterStatusQueryResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
