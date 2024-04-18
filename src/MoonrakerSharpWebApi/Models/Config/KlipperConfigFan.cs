using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigFan : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pin")]
        string pin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cycle_time")]
        double cycleTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("off_below")]
        long offBelow;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("shutdown_speed")]
        long shutdownSpeed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_power")]
        long maxPower;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("kick_start_time")]
        double kickStartTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("hardware_pwm")]
        bool hardwarePwm;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
