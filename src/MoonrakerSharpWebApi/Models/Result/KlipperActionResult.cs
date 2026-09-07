namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperActionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial string Result { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperActionResult);
        #endregion
    }
}
