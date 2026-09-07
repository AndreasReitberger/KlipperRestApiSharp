namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMetaRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperGcodeMetaResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperGcodeMetaRespone);
        #endregion
    }
}
