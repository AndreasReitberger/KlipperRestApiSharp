using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigFan : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("pin")]
        string pin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("cycle_time")]
        double cycleTime;

        [ObservableProperty]
        [property: JsonProperty("off_below")]
        long offBelow;

        [ObservableProperty]
        [property: JsonProperty("shutdown_speed")]
        long shutdownSpeed;

        [ObservableProperty]
        [property: JsonProperty("max_power")]
        long maxPower;

        [ObservableProperty]
        [property: JsonProperty("kick_start_time")]
        double kickStartTime;

        [ObservableProperty]
        [property: JsonProperty("hardware_pwm")]
        bool hardwarePwm;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
