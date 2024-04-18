using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMotionReport : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("live_position")]
        List<double> livePosition = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("steppers")]
        List<string> steppers = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("live_velocity")]
        double? liveVelocity;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("live_extruder_velocity")]
        double? liveExtruderVelocity;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("trapq")]
        List<string> trapq = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
