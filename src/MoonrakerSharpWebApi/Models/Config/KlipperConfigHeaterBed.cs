using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigHeaterBed
    {
        #region Properties
        [JsonProperty("control")]
        public string Control { get; set; } = string.Empty;

        [JsonProperty("pid_kp")]
        public double PidKp { get; set; }

        [JsonProperty("pullup_resistor")]
        public long PullupResistor { get; set; }

        [JsonProperty("sensor_pin")]
        public string SensorPin { get; set; } = string.Empty;

        [JsonProperty("heater_pin")]
        public string HeaterPin { get; set; } = string.Empty;

        [JsonProperty("max_power")]
        public long MaxPower { get; set; }

        [JsonProperty("min_extrude_temp")]
        public long MinExtrudeTemp { get; set; }

        [JsonProperty("sensor_type")]
        public string SensorType { get; set; } = string.Empty;

        [JsonProperty("inline_resistor")]
        public long InlineResistor { get; set; }

        [JsonProperty("pid_kd")]
        public double PidKd { get; set; }

        [JsonProperty("pwm_cycle_time")]
        public double PwmCycleTime { get; set; }

        [JsonProperty("pid_ki")]
        public double PidKi { get; set; }

        [JsonProperty("min_temp")]
        public long MinTemp { get; set; }

        [JsonProperty("max_temp")]
        public long MaxTemp { get; set; }

        [JsonProperty("smooth_time")]
        public long SmoothTime { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
