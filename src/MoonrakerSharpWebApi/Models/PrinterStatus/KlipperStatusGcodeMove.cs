using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusGcodeMove : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("homing_origin")]
        public partial List<double> HomingOrigin { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("speed_factor")]
        public partial double? SpeedFactor { get; set; }

        [ObservableProperty]

        [JsonProperty("gcode_position")]
        public partial List<double> GcodePosition { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("absolute_extrude")]
        public partial bool AbsoluteExtrude { get; set; }

        [ObservableProperty]

        [JsonProperty("absolute_coordinates")]
        public partial bool AbsoluteCoordinates { get; set; }

        [ObservableProperty]

        [JsonProperty("position")]
        public partial List<double> Position { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("speed")]
        public partial double? Speed { get; set; }

        [ObservableProperty]

        [JsonProperty("extrude_factor")]
        public partial double? ExtrudeFactor { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
