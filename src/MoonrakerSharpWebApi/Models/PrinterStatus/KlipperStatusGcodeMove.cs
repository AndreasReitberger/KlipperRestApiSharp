using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusGcodeMove : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("homing_origin")]
        List<double> homingOrigin = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("speed_factor")]
        double? speedFactor;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcode_position")]
        List<double> gcodePosition = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("absolute_extrude")]
        bool absoluteExtrude;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("absolute_coordinates")]
        bool absoluteCoordinates;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("position")]
        List<double> position = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("speed")]
        double? speed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extrude_factor")]
        double? extrudeFactor;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
