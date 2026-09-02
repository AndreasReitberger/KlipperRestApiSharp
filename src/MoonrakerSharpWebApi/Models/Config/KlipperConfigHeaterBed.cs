namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigHeaterBed : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("control")]
        public partial string Control { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("pid_kp")]
        public partial double PidKp { get; set; }

        [ObservableProperty]
        [JsonPropertyName("pullup_resistor")]
        public partial long PullupResistor { get; set; }

        [ObservableProperty]
        [JsonPropertyName("sensor_pin")]
        public partial string SensorPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("heater_pin")]
        public partial string HeaterPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("max_power")]
        public partial long MaxPower { get; set; }

        [ObservableProperty]
        [JsonPropertyName("min_extrude_temp")]
        public partial long MinExtrudeTemp { get; set; }

        [ObservableProperty]
        [JsonPropertyName("sensor_type")]
        public partial string SensorType { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("inline_resistor")]
        public partial long InlineResistor { get; set; }

        [ObservableProperty]
        [JsonPropertyName("pid_kd")]
        public partial double PidKd { get; set; }

        [ObservableProperty]
        [JsonPropertyName("pwm_cycle_time")]
        public partial double PwmCycleTime { get; set; }

        [ObservableProperty]
        [JsonPropertyName("pid_ki")]
        public partial double PidKi { get; set; }

        [ObservableProperty]
        [JsonPropertyName("min_temp")]
        public partial long MinTemp { get; set; }

        [ObservableProperty]
        [JsonPropertyName("max_temp")]
        public partial long MaxTemp { get; set; }

        [ObservableProperty]
        [JsonPropertyName("smooth_time")]
        public partial long SmoothTime { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperConfigHeaterBed);
        #endregion
    }
}
