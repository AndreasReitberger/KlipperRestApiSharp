using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusHeaters : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("available_sensors")]
        public partial List<string> AvailableSensors { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("available_heaters")]
        public partial List<string> AvailableHeaters { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperStatusHeaters);
        #endregion
    }
}
