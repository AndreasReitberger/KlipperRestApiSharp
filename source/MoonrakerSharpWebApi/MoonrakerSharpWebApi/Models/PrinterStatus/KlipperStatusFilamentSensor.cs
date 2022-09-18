using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusFilamentSensor
    {
        #region Properties
        [JsonProperty("filament_detected")]
        public bool FilamentDetected { get; set; }
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
