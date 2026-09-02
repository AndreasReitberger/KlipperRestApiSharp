using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateTemperature : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("bed")]
        public partial OctoprintApiPrinterStateTemperatureInfo? BBed { get; set; }

        [ObservableProperty]

        [JsonPropertyName("chamber")]
        public partial OctoprintApiPrinterStateTemperatureInfo? Chamber { get; set; }

        [ObservableProperty]

        [JsonPropertyName("history")]
        public partial List<OctoprintApiPrinterStateHistory> History { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("tool0")]
        public partial OctoprintApiPrinterStateTemperatureInfo? Tool0 { get; set; }

        [ObservableProperty]

        [JsonPropertyName("tool1")]
        public partial OctoprintApiPrinterStateTemperatureInfo? Tool1 { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
