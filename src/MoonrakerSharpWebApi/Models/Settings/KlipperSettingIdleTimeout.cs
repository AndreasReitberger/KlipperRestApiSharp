namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingIdleTimeout : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("gcode")]
        public partial string Gcode { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("timeout")]
        public partial long Timeout { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
