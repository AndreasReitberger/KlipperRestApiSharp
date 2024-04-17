using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusGcodeMove : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("homing_origin")]
        List<double> homingOrigin = new();

        [ObservableProperty]
        [property: JsonProperty("speed_factor")]
        double? speedFactor;

        [ObservableProperty]
        [property: JsonProperty("gcode_position")]
        List<double> gcodePosition = new();

        [ObservableProperty]
        [property: JsonProperty("absolute_extrude")]
        bool absoluteExtrude;

        [ObservableProperty]
        [property: JsonProperty("absolute_coordinates")]
        bool absoluteCoordinates;

        [ObservableProperty]
        [property: JsonProperty("position")]
        List<double> position = new();

        [ObservableProperty]
        [property: JsonProperty("speed")]
        double? speed;

        [ObservableProperty]
        [property: JsonProperty("extrude_factor")]
        double? extrudeFactor;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
