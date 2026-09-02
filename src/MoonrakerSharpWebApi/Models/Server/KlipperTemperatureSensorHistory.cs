using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperTemperatureSensorHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("temperatures")]
        public partial List<double> Temperatures { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("targets")]
        public partial List<long> Targets { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("powers")]
        public partial List<long> Powers { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
