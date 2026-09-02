namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUserActionRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperUserActionResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
