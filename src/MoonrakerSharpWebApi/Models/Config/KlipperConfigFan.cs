namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigFan : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("pin")]
        public partial string Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("cycle_time")]
        public partial double CycleTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("off_below")]
        public partial long OffBelow { get; set; }

        [ObservableProperty]

        [JsonPropertyName("shutdown_speed")]
        public partial long ShutdownSpeed { get; set; }

        [ObservableProperty]

        [JsonPropertyName("max_power")]
        public partial long MaxPower { get; set; }

        [ObservableProperty]

        [JsonPropertyName("kick_start_time")]
        public partial double KickStartTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("hardware_pwm")]
        public partial bool HardwarePwm { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperConfigFan);
        #endregion
    }
}
