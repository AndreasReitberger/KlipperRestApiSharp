using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusFilamentSensor
    {
        #region Properties
        [JsonProperty("filament_detected")]
        public bool FilamentDetected { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
