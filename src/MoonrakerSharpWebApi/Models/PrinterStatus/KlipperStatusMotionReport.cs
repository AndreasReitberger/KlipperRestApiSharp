using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMotionReport : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("live_position")]
        public partial List<double> LivePosition { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("steppers")]
        public partial List<string> Steppers { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("live_velocity")]
        public partial double? LiveVelocity { get; set; }

        [ObservableProperty]
        
        [JsonProperty("live_extruder_velocity")]
        public partial double? LiveExtruderVelocity { get; set; }

        [ObservableProperty]
        
        [JsonProperty("trapq")]
        public partial List<string> Trapq { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
