using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusMotionReport
    {
        #region Properties
        [JsonProperty("live_position")]
        public List<double> LivePosition { get; set; } = new();

        [JsonProperty("steppers")]
        public List<string> Steppers { get; set; } = new();

        [JsonProperty("live_velocity")]
        public double LiveVelocity { get; set; }

        [JsonProperty("live_extruder_velocity")]
        public double LiveExtruderVelocity { get; set; }

        [JsonProperty("trapq")]
        public List<string> Trapq { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
