namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusDriverRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("drv_status")]
        public partial KlipperStatusDriver? DrvStatus { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperStatusDriverRespone);
        #endregion
    }
}
