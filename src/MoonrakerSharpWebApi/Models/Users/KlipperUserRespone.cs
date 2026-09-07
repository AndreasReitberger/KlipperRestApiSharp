namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUserRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperUser? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperUserRespone);
        #endregion
    }
}
