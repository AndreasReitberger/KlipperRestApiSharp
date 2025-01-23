using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigFan : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("pin")]
        public partial string Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("cycle_time")]
        public partial double CycleTime { get; set; }

        [ObservableProperty]

        [JsonProperty("off_below")]
        public partial long OffBelow { get; set; }

        [ObservableProperty]

        [JsonProperty("shutdown_speed")]
        public partial long ShutdownSpeed { get; set; }

        [ObservableProperty]

        [JsonProperty("max_power")]
        public partial long MaxPower { get; set; }

        [ObservableProperty]

        [JsonProperty("kick_start_time")]
        public partial double KickStartTime { get; set; }

        [ObservableProperty]

        [JsonProperty("hardware_pwm")]
        public partial bool HardwarePwm { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
