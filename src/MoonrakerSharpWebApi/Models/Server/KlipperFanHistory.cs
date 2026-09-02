using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{

    public partial class KlipperFanHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("temperatures")]
        public partial List<double> Temperatures { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("targets")]
        public partial List<long> Targets { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("speeds")]
        public partial List<long> Speeds { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
