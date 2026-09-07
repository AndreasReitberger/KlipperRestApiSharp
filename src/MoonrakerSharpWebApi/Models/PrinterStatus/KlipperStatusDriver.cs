namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusDriver : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("cs_actual")]
        public partial long? CsActual { get; set; }

        [ObservableProperty]

        [JsonPropertyName("sg_result")]
        public partial long? SgResult { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperStatusDriver);
        #endregion
    }
}
