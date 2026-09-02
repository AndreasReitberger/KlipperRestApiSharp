using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusGcodeMove : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("homing_origin")]
        public partial List<double> HomingOrigin { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("speed_factor")]
        public partial double? SpeedFactor { get; set; }

        [ObservableProperty]

        [JsonPropertyName("gcode_position")]
        public partial List<double> GcodePosition { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("absolute_extrude")]
        public partial bool AbsoluteExtrude { get; set; }

        [ObservableProperty]

        [JsonPropertyName("absolute_coordinates")]
        public partial bool AbsoluteCoordinates { get; set; }

        [ObservableProperty]

        [JsonPropertyName("position")]
        public partial List<double> Position { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("speed")]
        public partial double? Speed { get; set; }

        [ObservableProperty]

        [JsonPropertyName("extrude_factor")]
        public partial double? ExtrudeFactor { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
