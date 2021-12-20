using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusToolhead
    {
        #region Properties
        [JsonProperty("square_corner_velocity")]
        public long SquareCornerVelocity { get; set; }

        [JsonProperty("max_accel")]
        public long MaxAccel { get; set; }

        [JsonProperty("homed_axes")]
        public string HomedAxes { get; set; }

        [JsonProperty("estimated_print_time")]
        public double EstimatedPrintTime { get; set; }

        [JsonProperty("max_velocity")]
        public long MaxVelocity { get; set; }

        [JsonProperty("print_time")]
        public double PrintTime { get; set; }

        [JsonProperty("max_accel_to_decel")]
        public long MaxAccelToDecel { get; set; }

        [JsonProperty("axis_minimum")]
        public List<long> AxisMinimum { get; set; } = new();

        [JsonProperty("stalls")]
        public long Stalls { get; set; }

        [JsonProperty("axis_maximum")]
        public List<long> AxisMaximum { get; set; } = new();

        [JsonProperty("position")]
        public List<long> Position { get; set; } = new();

        [JsonProperty("extruder")]
        public string Extruder { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
