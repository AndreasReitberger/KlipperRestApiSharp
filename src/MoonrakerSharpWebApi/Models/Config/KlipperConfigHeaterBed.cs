using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigHeaterBed : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("control")]
        public partial string Control { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("pid_kp")]
        public partial double PidKp { get; set; }

        [ObservableProperty]

        [JsonProperty("pullup_resistor")]
        public partial long PullupResistor { get; set; }

        [ObservableProperty]

        [JsonProperty("sensor_pin")]
        public partial string SensorPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("heater_pin")]
        public partial string HeaterPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("max_power")]
        public partial long MaxPower { get; set; }

        [ObservableProperty]

        [JsonProperty("min_extrude_temp")]
        public partial long MinExtrudeTemp { get; set; }

        [ObservableProperty]

        [JsonProperty("sensor_type")]
        public partial string SensorType { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("inline_resistor")]
        public partial long InlineResistor { get; set; }

        [ObservableProperty]

        [JsonProperty("pid_kd")]
        public partial double PidKd { get; set; }

        [ObservableProperty]

        [JsonProperty("pwm_cycle_time")]
        public partial double PwmCycleTime { get; set; }

        [ObservableProperty]

        [JsonProperty("pid_ki")]
        public partial double PidKi { get; set; }

        [ObservableProperty]

        [JsonProperty("min_temp")]
        public partial long MinTemp { get; set; }

        [ObservableProperty]

        [JsonProperty("max_temp")]
        public partial long MaxTemp { get; set; }

        [ObservableProperty]

        [JsonProperty("smooth_time")]
        public partial long SmoothTime { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
