namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMacroRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperGcodeMacroResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperGcodeMacroRespone);
        #endregion
    }
}
