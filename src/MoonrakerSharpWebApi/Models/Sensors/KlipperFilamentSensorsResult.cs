using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFilamentSensorsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("status")]
        public partial Dictionary<string, KlipperStatusFilamentSensor> Status { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("eventtime")]
        public partial double? Eventtime { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
