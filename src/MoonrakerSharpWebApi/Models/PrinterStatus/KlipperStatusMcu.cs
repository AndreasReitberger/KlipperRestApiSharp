using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMcu : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("mcu_build_versions")]
        public partial string McuBuildVersions { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("mcu_version")]
        public partial string McuVersion { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("last_stats")]
        public partial Dictionary<string, double> LastStats { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("mcu_constants")]
        public partial Dictionary<string, object> McuConstants { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
