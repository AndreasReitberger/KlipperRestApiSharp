namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodesRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperGcodesResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperGcodesRespone);
        #endregion
    }
}
