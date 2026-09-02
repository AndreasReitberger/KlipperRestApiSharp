using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("cameras")]
        public partial List<KlipperDatabaseFluiddValueWebcamConfig> Cameras { get; set; } = [];

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseFluiddValueWebcam);
        #endregion
    }
}
