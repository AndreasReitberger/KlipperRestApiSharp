using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusGcodeMove
    {
        #region Properties
        [JsonProperty("homing_origin")]
        public List<double> HomingOrigin { get; set; } = new();

        [JsonProperty("speed_factor")]
        public double SpeedFactor { get; set; }

        [JsonProperty("gcode_position")]
        public List<double> GcodePosition { get; set; } = new();

        [JsonProperty("absolute_extrude")]
        public bool AbsoluteExtrude { get; set; }

        [JsonProperty("absolute_coordinates")]
        public bool AbsoluteCoordinates { get; set; }

        [JsonProperty("position")]
        public List<double> Position { get; set; } = new();

        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("extrude_factor")]
        public double ExtrudeFactor { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
