using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMotionReport : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("live_position")]
        public partial List<double> LivePosition { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("steppers")]
        public partial List<string> Steppers { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("live_velocity")]
        public partial double? LiveVelocity { get; set; }

        [ObservableProperty]

        [JsonPropertyName("live_extruder_velocity")]
        public partial double? LiveExtruderVelocity { get; set; }

        [ObservableProperty]

        [JsonPropertyName("trapq")]
        public partial List<string> Trapq { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperStatusMotionReport);
        #endregion
    }
}
