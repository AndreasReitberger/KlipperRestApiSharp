using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigFan
    {
        #region Properties
        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("cycle_time")]
        public double CycleTime { get; set; }

        [JsonProperty("off_below")]
        public long OffBelow { get; set; }

        [JsonProperty("shutdown_speed")]
        public long ShutdownSpeed { get; set; }

        [JsonProperty("max_power")]
        public long MaxPower { get; set; }

        [JsonProperty("kick_start_time")]
        public double KickStartTime { get; set; }

        [JsonProperty("hardware_pwm")]
        public bool HardwarePwm { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
