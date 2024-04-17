using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMotionReport : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("live_position")]
        List<double> livePosition = new();

        [ObservableProperty]
        [property: JsonProperty("steppers")]
        List<string> steppers = new();

        [ObservableProperty]
        [property: JsonProperty("live_velocity")]
        double? liveVelocity;

        [ObservableProperty]
        [property: JsonProperty("live_extruder_velocity")]
        double? liveExtruderVelocity;

        [ObservableProperty]
        [property: JsonProperty("trapq")]
        List<string> trapq = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
