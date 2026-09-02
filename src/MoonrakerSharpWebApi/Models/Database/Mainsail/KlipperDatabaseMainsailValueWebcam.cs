using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("configs")]
        public partial List<KlipperDatabaseMainsailValueWebcamConfig> Configs { get; set; } = [];

        [ObservableProperty]
        [JsonPropertyName("boolNavi")]
        public partial bool BoolNavi { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
