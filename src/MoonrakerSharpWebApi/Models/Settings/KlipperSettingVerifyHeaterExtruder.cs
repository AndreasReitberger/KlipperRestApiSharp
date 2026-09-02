namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingVerifyHeaterExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("max_error")]
        public partial long MaxError { get; set; }

        [ObservableProperty]

        [JsonPropertyName("check_gain_time")]
        public partial long CheckGainTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("heating_gain")]
        public partial long HeatingGain { get; set; }

        [ObservableProperty]

        [JsonPropertyName("hysteresis")]
        public partial long Hysteresis { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
