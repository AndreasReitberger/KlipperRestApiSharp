using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusGcodeMove
    {
        #region Properties
        [JsonProperty("homing_origin")]
        public List<long> HomingOrigin { get; set; } = new();

        [JsonProperty("speed_factor")]
        public long SpeedFactor { get; set; }

        [JsonProperty("gcode_position")]
        public List<long> GcodePosition { get; set; } = new();

        [JsonProperty("absolute_extrude")]
        public bool AbsoluteExtrude { get; set; }

        [JsonProperty("absolute_coordinates")]
        public bool AbsoluteCoordinates { get; set; }

        [JsonProperty("position")]
        public List<long> Position { get; set; } = new();

        [JsonProperty("speed")]
        public long Speed { get; set; }

        [JsonProperty("extrude_factor")]
        public long ExtrudeFactor { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
