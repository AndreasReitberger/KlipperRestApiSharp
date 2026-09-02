namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFilamentSensorsRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperFilamentSensorsResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
