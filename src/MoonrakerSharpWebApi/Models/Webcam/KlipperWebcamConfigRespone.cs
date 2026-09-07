namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebcamConfigRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperWebcamConfigResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperWebcamConfigRespone);

        #endregion
    }
}
